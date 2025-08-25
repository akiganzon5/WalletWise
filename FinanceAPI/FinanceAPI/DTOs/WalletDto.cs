using System.ComponentModel.DataAnnotations;

namespace FinanceAPI.DTOs
{
    public class CreateWalletDto
    {
        [Required]
        public int Income { get; set; }
        [Required]
        public DateTime Month { get; set; } 
    }

    public class WalletDto
    {
        public int Id { get; set; }
        public int Income { get; set; }
        public DateTime Month { get; set; }
        public string MonthFormatted => Month.ToString("MMMM yyyy");
        public int TotalExpenses { get; set; }
        public int Balance => Income - TotalExpenses;
    }

    public class WalletWithExpensesDto
    {
        public int Id { get; set; }
        public int Income { get; set; }
        public DateTime Month { get; set; }

        public int TotalExpenses { get; set; }
        public int Balance => Income - TotalExpenses;
        public string MonthFormatted => Month.ToString("MMMM yyyy");

        public List<ExpenseDto> Expenses { get; set; }
        public DateTime LastUpdatedDate => DateTime.Now.Date;

    }

}
