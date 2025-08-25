import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ExpenseDto, CreateExpenseDto, UpdateExpenseDto } from '../../model/ExpenseDto';
import { CategoryService } from '../../services/category.service';
import { CategoryDto } from '../../model/CategoryDto ';


@Component({
  selector: 'app-expense-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './expense-modal.component.html',
  styleUrl: './expense-modal.component.css'
})
export class ExpenseModalComponent implements OnChanges, OnInit {
  @Input() visible = false;
  @Input() walletId!: number;
  @Input() expense: ExpenseDto | null = null; 
  @Output() save = new EventEmitter<CreateExpenseDto | UpdateExpenseDto>();
  @Output() close = new EventEmitter<void>();

  form!: FormGroup;
  isEdit = false;
  categories: CategoryDto[] = []; 

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.categoryService.getAllCategory().subscribe({
      next: (data) => this.categories = data,
      error: (err) => console.error('Failed to load categories', err)
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['expense'] && this.expense) {
      this.isEdit = true;
      this.form = this.fb.group({
        description: [this.expense.description, Validators.required],
        amount: [this.expense.amount, [Validators.required, Validators.min(1)]],
        date: [this.expense.date, Validators.required],
        categoryId: [this.expense.categoryId, Validators.required]
      });
    } else {
      this.isEdit = false;
      this.form = this.fb.group({
        description: ['', Validators.required],
        amount: [null, [Validators.required, Validators.min(1)]],
        date: [new Date().toISOString().substring(0, 10), Validators.required],
        categoryId: [null, Validators.required]
      });
    }
  }

  onSubmit() {
    if (this.form.valid) {
      const dto = {
        walletId: this.walletId,
        ...this.form.value
      };
      this.save.emit(dto);
    }
  }

  onClose() {
    this.close.emit();
  }
}
