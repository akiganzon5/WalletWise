import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryDto } from '../../model/CategoryDto ';

@Component({
  selector: 'app-category-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category-modal.component.html',
  styleUrls: ['./category-modal.component.css']
})
export class CategoryModalComponent {
  @Input() show = false;
  @Input() isEdit = false;
  @Input() category: CategoryDto | null = null;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<{ name: string; id?: number }>();

  formData: { name: string } = { name: '' };

  ngOnChanges() {
    this.formData.name = this.category?.name ?? '';
  }

  onSave() {
    if (!this.formData.name.trim()) return;
    this.save.emit({
      id: this.category?.id,
      name: this.formData.name.trim()
    });
  }

  onClose() {
    this.close.emit();
  }
}
