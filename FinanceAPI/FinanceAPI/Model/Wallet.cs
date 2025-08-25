namespace FinanceAPI.Model
{
    public class Wallet
    {
        public int Id { get; set; }
        public int Income { get; set; }
        public DateTime Month { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
