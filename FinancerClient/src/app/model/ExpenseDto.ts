export interface ExpenseDto {
  id: number;
  description: string;
  amount: number;
  date: string; 
  categoryName: string;
  categoryId: number;
  walletId: number;
}

export interface CreateExpenseDto {
  walletId: number;
  description?: string;
  amount: number;
  categoryId: number;
}

export interface UpdateExpenseDto {
  description?: string;
  amount: number;
  categoryId: number;
}
