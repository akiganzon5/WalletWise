﻿namespace FinanceAPI.Model
{
    public class Expense
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
