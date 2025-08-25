using AutoMapper;
using FinanceAPI.DTOs;
using FinanceAPI.Model;
using System;

namespace FinanceAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Expense
            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));

            // Enforce UTC for Expense.Date when creating/updating
            CreateMap<CreateExpenseDto, Expense>()
                .ForMember(dest => dest.Date,
                           opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateExpenseDto, Expense>()
                .ForMember(dest => dest.Date,
                           opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Wallet → WalletDto
            CreateMap<Wallet, WalletDto>()
                .ForMember(dest => dest.TotalExpenses,
                           opt => opt.MapFrom(src => src.Expenses != null
                               ? src.Expenses.Sum(e => e.Amount)
                               : 0));

            // Enforce UTC for Wallet.Month when creating
            CreateMap<CreateWalletDto, Wallet>()
                .ForMember(dest => dest.Month,
                           opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Month, DateTimeKind.Utc)));

            // Wallet → WalletWithExpensesDto
            CreateMap<Wallet, WalletWithExpensesDto>()
                .ForMember(dest => dest.TotalExpenses,
                           opt => opt.MapFrom(src => src.Expenses != null
                               ? src.Expenses.Sum(e => e.Amount)
                               : 0))
                .ForMember(dest => dest.Expenses,
                           opt => opt.MapFrom(src =>
                               (src.Expenses ?? new List<Expense>())
                               .OrderByDescending(e => e.Date)));
        }
    }
}
