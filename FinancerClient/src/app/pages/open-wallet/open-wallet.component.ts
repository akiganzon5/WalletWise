import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WalletWithExpensesDto } from '../../model/WalletDto';
import { LucideAngularModule, X } from 'lucide-angular';

@Component({
  selector: 'app-open-wallet',
  standalone: true,
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './open-wallet.component.html',
  styleUrls: ['./open-wallet.component.css']
})
export class OpenWalletComponent implements OnChanges {
  @Input() wallet!: WalletWithExpensesDto;
  @Input() visible = false;   // control modal open/close
  @Input() onClose!: () => void;

  X = X;

  monthFormatted: string = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['wallet'] && this.wallet) {
      this.monthFormatted = new Date(this.wallet.month)
        .toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
    }
  }

  // Helper method to calculate expense percentage for progress bar
  getExpensePercentage(): number {
    if (!this.wallet || this.wallet.income <= 0) return 0;
    return Math.min(100, (this.wallet.totalExpenses / this.wallet.income) * 100);
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