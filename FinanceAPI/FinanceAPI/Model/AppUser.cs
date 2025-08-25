using Microsoft.AspNetCore.Identity;

namespace FinanceAPI.Model
{
    public class AppUser : IdentityUser
    {
        public ICollection<Wallet> Wallets { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
