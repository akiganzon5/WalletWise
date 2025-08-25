import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreateWalletDto } from '../../model/WalletDto';

@Component({
  selector: 'app-wallet-modal',
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './wallet-modal.component.html',
  styleUrl: './wallet-modal.component.css'
})
export class WalletModalComponent {
  @Input() visible = false;
  @Output() save = new EventEmitter<CreateWalletDto>();
  @Output() close = new EventEmitter<void>();

  walletForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.walletForm = this.fb.group({
      income: [null, [Validators.required, Validators.min(1)]],
      month: [null, Validators.required]
    });
  }

  onSubmit() {
    if (this.walletForm.valid) {
      const rawValue = this.walletForm.value;
      const dto: CreateWalletDto = {
        income: rawValue.income,
        month: rawValue.month + "-01" 
      };

      this.save.emit(dto);
    }
  }

  onClose() {
    this.close.emit();
  }
}
