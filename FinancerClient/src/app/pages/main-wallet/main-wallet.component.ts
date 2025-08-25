import { Component, OnInit } from '@angular/core';
import { WalletService } from '../../services/wallet.service';
import { ExpenseService } from '../../services/expense.service';
import { WalletWithExpensesDto } from '../../model/WalletDto';
import { ExpenseDto, CreateExpenseDto, UpdateExpenseDto } from '../../model/ExpenseDto';
import { CommonModule } from '@angular/common';
import { LucideAngularModule, Plus, Trash2, Edit, Archive } from 'lucide-angular';
import { ExpenseModalComponent } from '../expense-modal/expense-modal.component';
import { WalletModalComponent } from '../wallet-modal/wallet-modal.component';
import { ConfirmationModalComponent } from '../confirmation-modal/confirmation-modal.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-main-wallet',
  standalone: true,
  imports: [
    CommonModule, 
    LucideAngularModule, 
    ExpenseModalComponent, 
    WalletModalComponent,
    ConfirmationModalComponent
  ],
  templateUrl: './main-wallet.component.html',
  styleUrl: './main-wallet.component.css'
})
export class MainWalletComponent implements OnInit {
  wallet: WalletWithExpensesDto | null = null;
  loading = true;
  error: string | null = null;

  showExpenseModal = false;
  selectedExpense: ExpenseDto | null = null;

  showWalletModal = false; 

  showConfirmDelete = false;
  showConfirmArchive = false;
  expenseToDelete: number | null = null;

  Plus = Plus;
  Trash2 = Trash2;
  Edit = Edit;
  Archive = Archive;

  constructor(
    private walletService: WalletService, 
    private expenseService: ExpenseService,
    private toastr: ToastrService  
  ) {}

  ngOnInit(): void {
    this.loadWallet();
  }

  // Load latest wallet from API
  loadWallet() {
    this.walletService.getLatestWallet().subscribe({
      next: (data) => {
        this.wallet = data;
        this.loading = false;
      },
      error: (err) => {
        if (err.status === 404) {
          this.wallet = null;
        } else {
          this.error = 'Something went wrong loading your wallet.';
        }
        this.loading = false;
      }
    });
  }

  // 🔹 Open wallet modal
  onAddWallet() {
    this.showWalletModal = true;
  }

  // 🔹 Handle wallet save
  handleSaveWallet(dto: { income: number; month: string }) {
    this.walletService.createWallet(dto).subscribe(() => {
      this.showWalletModal = false;
      this.loadWallet();
      this.toastr.success('Wallet created successfully!', 'Success ✅');
    });
  }

  // Expense actions
  onAddExpense() {
    this.selectedExpense = null;
    this.showExpenseModal = true;
  }

  onEditExpense(expenseId: number) {
    this.selectedExpense = this.wallet?.expenses.find(e => e.id === expenseId) || null;
    this.showExpenseModal = true;
  }

  handleSaveExpense(dto: CreateExpenseDto | UpdateExpenseDto) {
    if (this.selectedExpense) {
      this.expenseService.updateExpense(this.selectedExpense.id, dto).subscribe(() => {
        this.loadWallet();
        this.showExpenseModal = false;
        this.toastr.success('Expense updated successfully!', 'Updated ✅');
      });
    } else {
      this.expenseService.createExpense(dto as CreateExpenseDto).subscribe(() => {
        this.loadWallet();
        this.showExpenseModal = false;
        this.toastr.success('Expense added successfully!', 'Created ✅');
      });
    }
  }

  confirmDeleteExpense(id: number) {
    this.expenseToDelete = id;
    this.showConfirmDelete = true;
  }

  deleteExpenseConfirmed() {
    if (this.expenseToDelete) {
      this.expenseService.deleteExpense(this.expenseToDelete).subscribe(() => {
        this.loadWallet();
        this.showConfirmDelete = false;
        this.expenseToDelete = null;
        this.toastr.info('Expense deleted successfully', 'Deleted 🗑️'); 
      });
    }
  }

  confirmArchiveWallet() {
    this.showConfirmArchive = true;
  }

  archiveWalletConfirmed() {
    if (this.wallet) {
      this.walletService.archiveWallet(this.wallet.id).subscribe(() => {
        this.wallet = null;
        this.showConfirmArchive = false;
        this.toastr.success('Wallet archived successfully!', 'Archived 📦');
      });
    }
  }

  // Helper method to calculate expense percentage for progress bar
  getExpensePercentage(wallet: WalletWithExpensesDto): number {
    if (wallet.income <= 0) return 0;
    return Math.min(100, (wallet.totalExpenses / wallet.income) * 100);
  }

  // Helper method to generate category colors
  getCategoryColor(category: string): string {
    const colors = [
      '#4f46e5', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6', 
      '#ec4899', '#06b6d4', '#84cc16', '#f97316', '#6366f1'
    ];
    
    let hash = 0;
    for (let i = 0; i < category.length; i++) {
      hash = category.charCodeAt(i) + ((hash << 5) - hash);
    }
    
    return colors[Math.abs(hash) % colors.length];
  }
}