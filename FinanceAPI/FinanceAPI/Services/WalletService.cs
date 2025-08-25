using AutoMapper;
using FinanceAPI.DTOs;
using FinanceAPI.Model;
using FinanceAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceAPI.Services
{
    public class WalletService
    {
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WalletService(
            IRepository<Wallet> walletRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WalletDto>> GetAllWalletsAsync()
        {
            var userId = GetCurrentUserId();
            var wallets = await _walletRepository.FindAsync(w => w.UserId == userId);
            return _mapper.Map<IEnumerable<WalletDto>>(wallets);
        }

        public async Task<WalletDto?> GetWalletByIdAsync(int id)
        {
            var userId = GetCurrentUserId();
            var wallet = await _walletRepository.FirstOrDefaultAsync(
                w => w.Id == id && w.UserId == userId
            );

            return wallet == null ? null : _mapper.Map<WalletDto>(wallet);
        }

        public async Task<WalletWithExpensesDto?> GetWalletWithExpensesByIdAsync(int walletId)
        {
            var userId = GetCurrentUserId();

            var wallet = await _walletRepository.FirstOrDefaultWithIncludesAsync(
                w => w.Id == walletId && w.UserId == userId,
                q => q.Include(w => w.Expenses)
                      .ThenInclude(e => e.Category)
             );

            if (wallet == null)
                return null;

            return _mapper.Map<WalletWithExpensesDto>(wallet);
        }


        public async Task<WalletDto> AddWalletAsync(CreateWalletDto dto)
        {
            var wallet = _mapper.Map<Wallet>(dto);
            wallet.UserId = GetCurrentUserId();

            await _walletRepository.AddAsync(wallet);
            return _mapper.Map<WalletDto>(wallet);
        }

        public async Task<bool> DeleteWalletAsync(int id)
        {
            var userId = GetCurrentUserId();
            var wallet = await _walletRepository.FirstOrDefaultAsync(
                w => w.Id == id && w.UserId == userId
            );

            if (wallet == null)
                return false;

            await _walletRepository.DeleteAsync(wallet.Id);
            return true;
        }

        public async Task ArchiveWalletAsync(int walletId)
        {
            var userId = GetCurrentUserId();

            var wallet = await _walletRepository.FirstOrDefaultAsync(
                w => w.Id == walletId && w.UserId == userId
            );

            if (wallet == null)
                throw new Exception("Wallet not found");

            wallet.IsArchived = true;
            await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<IEnumerable<WalletWithExpensesDto>> GetArchivedWalletsAsync()
        {
            var userId = GetCurrentUserId();

            var wallets = await _walletRepository.FindWithIncludesAsync(
                w => w.UserId == userId && w.IsArchived,
                q => q.Include(w => w.Expenses)
                      .ThenInclude(e => e.Category)
            );

            var orderedWallets = wallets.OrderByDescending(w => w.Month);

            return _mapper.Map<IEnumerable<WalletWithExpensesDto>>(orderedWallets);
        }


        public async Task<WalletWithExpensesDto?> GetLatestUnarchivedWalletAsync()
        {
            var userId = GetCurrentUserId();

            var wallets = await _walletRepository.FindWithIncludesAsync(
                w => w.UserId == userId && !w.IsArchived,
                q => q.Include(w => w.Expenses)
                      .ThenInclude(e => e.Category)
            );

            var latestWallet = wallets.OrderByDescending(w => w.Month).FirstOrDefault();

            return latestWallet == null ? null : _mapper.Map<WalletWithExpensesDto>(latestWallet);
        }


        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}
