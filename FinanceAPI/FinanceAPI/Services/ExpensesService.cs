using AutoMapper;
using FinanceAPI.DTOs;
using FinanceAPI.Model;
using FinanceAPI.Repository;
using System.Security.Claims;

namespace FinanceAPI.Services
{
    public class ExpenseService
    {
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ExpenseService(
            IRepository<Expense> expenseRepository,
            IRepository<Wallet> walletRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _walletRepository = walletRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ExpenseDto> AddExpenseAsync(CreateExpenseDto dto)
        {
            var userId = GetCurrentUserId();

            var wallet = await _walletRepository.FirstOrDefaultAsync(
                w => w.Id == dto.WalletId && w.UserId == userId
            );

            if (wallet == null)
                throw new UnauthorizedAccessException("Wallet not found or does not belong to the user.");

            var expense = _mapper.Map<Expense>(dto);
            expense.UserId = userId;
            expense.Date = DateTime.UtcNow;

            await _expenseRepository.AddAsync(expense);
            return _mapper.Map<ExpenseDto>(expense);
        }


        public async Task<IEnumerable<ExpenseDto>> GetAllExpensesAsync()
        {
            var userId = GetCurrentUserId();

            var expenses = await _expenseRepository.FindAsync(
                e => e.Wallet.UserId == userId,
                e => e.Category,
                e => e.Wallet
            );

            return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        }

        public async Task<ExpenseDto?> GetExpenseByIdAsync(int id)
        {
            var userId = GetCurrentUserId();

            var expense = await _expenseRepository.FirstOrDefaultAsync(
                e => e.Id == id && e.Wallet.UserId == userId,
                e => e.Category,
                e => e.Wallet
            );

            return expense == null ? null : _mapper.Map<ExpenseDto>(expense);
        }


        public async Task<ExpenseDto?> UpdateExpenseAsync(int id, UpdateExpenseDto dto)
        {
            var userId = GetCurrentUserId();

            var expense = await _expenseRepository.FirstOrDefaultAsync(
                e => e.Id == id && e.UserId == userId
            );

            if (expense == null)
                return null;

            _mapper.Map(dto, expense);
            expense.Date = DateTime.UtcNow;
            await _expenseRepository.UpdateAsync(expense);

            return _mapper.Map<ExpenseDto>(expense);
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var userId = GetCurrentUserId();

            var expense = await _expenseRepository.FirstOrDefaultAsync(
                e => e.Id == id && e.UserId == userId
            );

            if (expense == null)
                return false;

            await _expenseRepository.DeleteAsync(expense.Id);
            return true;
        }

        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}
