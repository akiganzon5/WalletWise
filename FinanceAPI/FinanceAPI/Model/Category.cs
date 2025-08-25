namespace FinanceAPI.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        public ICollection<Expense> Expenses { get; set; }
    }

}
