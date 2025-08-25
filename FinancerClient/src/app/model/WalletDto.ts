import { ExpenseDto } from './ExpenseDto';

export interface WalletDto {
  id: number;
  income: number;
  month: string;
  totalExpenses: number;
  balance: number;
  monthFormatted: string;
}

export interface WalletWithExpensesDto {
  id: number;
  income: number;
  month: string;
  totalExpenses: number;
  balance: number;
  expenses: ExpenseDto[];
  lastUpdatedDate: string | null;
}

export interface CreateWalletDto {
  income: number;
  month: string;
}
