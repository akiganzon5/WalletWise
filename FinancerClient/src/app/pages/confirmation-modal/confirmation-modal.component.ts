// confirmation-modal.component.ts
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirmation-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.css']
})
export class ConfirmationModalComponent {
  @Input() visible = false;                   // show/hide modal
  @Input() title = 'Confirm Action';          // title of modal
  @Input() message = 'Are you sure you want to proceed?'; // body message
  @Input() confirmText = 'Confirm';           // confirm button text
  @Input() cancelText = 'Cancel';             // cancel button text
  @Input() type: 'default' | 'danger' | 'warning' = 'default'; // modal type

  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onConfirm() {
    this.confirm.emit();
    this.visible = false;
  }

  onCancel() {
    this.cancel.emit();
    this.visible = false;
  }
}