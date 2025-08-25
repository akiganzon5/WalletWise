using System.ComponentModel.DataAnnotations;

namespace FinanceAPI.DTOs
{

    public class CreateExpenseDto
    {
        public int WalletId { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }

    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int WalletId { get; set; }
    }

    public class UpdateExpenseDto
    {
        public string? Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
