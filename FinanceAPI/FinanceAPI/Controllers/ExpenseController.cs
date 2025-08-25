using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using FinanceAPI.Services;
using FinanceAPI.DTOs;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateExpenseDto>> CreateExpense(CreateExpenseDto dto)
        {
            var expense = await _expenseService.AddExpenseAsync(dto);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpenseById(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateExpenseDto>> UpdateExpense(int id, UpdateExpenseDto dto)
        {
            var updatedExpense = await _expenseService.UpdateExpenseAsync(id, dto);
            if (updatedExpense == null)
                return NotFound();

            return Ok(updatedExpense);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var deletedExpense = await _expenseService.DeleteExpenseAsync(id);
            if (!deletedExpense)
                return NotFound();

            return NoContent();
        }
    }
}
