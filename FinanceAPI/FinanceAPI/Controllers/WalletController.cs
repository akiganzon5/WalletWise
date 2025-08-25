using FinanceAPI.DTOs;
using FinanceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateWalletDto>> CreateWallet(CreateWalletDto dto)
        {
            var wallet = await _walletService.AddWalletAsync(dto);
            return CreatedAtAction(nameof(GetWalletById), new { id = wallet.Id }, wallet);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletDto>>> GetAllWallet()
        {
            var wallets = await _walletService.GetAllWalletsAsync();
            return Ok(wallets);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<WalletWithExpensesDto>> GetWalletWithExpenses(int id)
        {
            var wallet = await _walletService.GetWalletWithExpensesByIdAsync(id);
            if (wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        [HttpGet("latest")]
        public async Task<ActionResult<WalletWithExpensesDto>> GetLatestWallet()
        {
            var wallet = await _walletService.GetLatestUnarchivedWalletAsync();
            if (wallet == null)
                return NotFound();
            return Ok(wallet);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletDto>> GetWalletById(int id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            if (wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var deletedWallet = await _walletService.DeleteWalletAsync(id);
            if (!deletedWallet)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> ArchiveWallet(int id)
        {
            await _walletService.ArchiveWalletAsync(id);
            return NoContent();
        }

        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedWallets()
        {
            var wallets = await _walletService.GetArchivedWalletsAsync();
            return Ok(wallets);
        }

    }
}
