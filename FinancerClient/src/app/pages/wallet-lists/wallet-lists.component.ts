import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletService } from '../../services/wallet.service';
import { WalletDto } from '../../model/WalletDto';
import { ToastrService } from 'ngx-toastr';
import { LucideAngularModule, Trash2 } from 'lucide-angular';
import { ConfirmationModalComponent } from '../confirmation-modal/confirmation-modal.component';
import { OpenWalletComponent } from '../open-wallet/open-wallet.component';
import { WalletWithExpensesDto } from '../../model/WalletDto';

@Component({
  selector: 'app-wallet-lists',
  standalone: true,
  imports: [
    CommonModule, 
    LucideAngularModule, 
    ConfirmationModalComponent,
    OpenWalletComponent
  ],
  templateUrl: './wallet-lists.component.html',
  styleUrl: './wallet-lists.component.css'
})
export class WalletListsComponent implements OnInit {
  wallets: WalletDto[] = [];
  loading = true;
  error: string | null = null;

  Trash2 = Trash2;

  showConfirmDelete = false;
  walletToDelete: number | null = null;

  selectedWallet: WalletWithExpensesDto | null = null;
  showOpenWallet = false;
  constructor(
    private walletService: WalletService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadWallets();
  }

  loadWallets() {
    this.walletService.getArchivedWallets().subscribe({
      next: (data) => {
        this.wallets = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load archived wallets.';
        this.loading = false;
      }
    });
  }

  goToWallet(wallet: WalletDto) {
    this.walletService.getWalletWithExpensesById(wallet.id).subscribe({
      next: (data) => {
        this.selectedWallet = data;
        this.showOpenWallet = true;
      }
    });
  }

  closeOpenWallet = () => {
    this.showOpenWallet = false;
    this.selectedWallet = null;
  };

  confirmDeleteWallet(id: number) {
    this.walletToDelete = id;
    this.showConfirmDelete = true;
  }

  deleteWalletConfirmed() {
    if (this.walletToDelete) {
      this.walletService.deleteWallet(this.walletToDelete).subscribe({
        next: () => {
          this.wallets = this.wallets.filter(w => w.id !== this.walletToDelete);
          this.toastr.success('Wallet deleted successfully!', 'Deleted 🗑️');
          this.showConfirmDelete = false;
          this.walletToDelete = null;
        }
      });
    }
  }

  // Helper method to calculate expense percentage for progress bar
  getExpensePercentage(wallet: WalletDto): number {
    if (wallet.income <= 0) return 0;
    return Math.min(100, (wallet.totalExpenses / wallet.income) * 100);
  }
}