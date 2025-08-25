import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../services/category.service';
import { CategoryDto } from '../../model/CategoryDto ';
import { ToastrService } from 'ngx-toastr';
import { LucideAngularModule, Edit, Trash2, Plus } from 'lucide-angular';
import { ConfirmationModalComponent } from '../confirmation-modal/confirmation-modal.component';
import { CategoryModalComponent } from '../category-modal/category-modal.component';

@Component({
  selector: 'app-category-lists',
  standalone: true,
  imports: [
    CommonModule,
    LucideAngularModule,
    ConfirmationModalComponent,
    CategoryModalComponent
  ],
  templateUrl: './category-lists.component.html',
  styleUrls: ['./category-lists.component.css']
})
export class CategoryListsComponent implements OnInit {
  categories: CategoryDto[] = [];
  loading = true;
  error: string | null = null;

  Trash2 = Trash2;
  Edit = Edit;
  Plus = Plus;

  showConfirmDelete = false;
  categoryToDelete: number | null = null;

  showCategoryModal = false;
  isEdit = false;
  editingCategory: CategoryDto | null = null;

  constructor(
    private categoryService: CategoryService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAllCategory().subscribe({
      next: (data) => {
        this.categories = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load categories.';
        this.loading = false;
      }
    });
  }

  openAddCategory() {
    this.isEdit = false;
    this.editingCategory = null;
    this.showCategoryModal = true;
  }

  openEditCategory(cat: CategoryDto) {
    this.isEdit = true;
    this.editingCategory = { ...cat };
    this.showCategoryModal = true;
  }

  saveCategory(data: { id?: number; name: string }) {
    if (this.isEdit && data.id) {
      this.categoryService.updateCategory(data.id, { name: data.name }).subscribe({
        next: () => {
          this.categories = this.categories.map(c =>
            c.id === data.id ? { ...c, name: data.name } : c
          );
          this.toastr.success('Category updated successfully!', 'Updated');
          this.showCategoryModal = false;
        }
      });
    } else {
      this.categoryService.createCategory({ name: data.name }).subscribe({
        next: (cat) => {
          this.categories.push(cat);
          this.toastr.success('Category created successfully!', 'Success');
          this.showCategoryModal = false;
        }
      });
    }
  }

  confirmDeleteCategory(id: number) {
    this.categoryToDelete = id;
    this.showConfirmDelete = true;
  }

  deleteCategoryConfirmed() {
    if (this.categoryToDelete) {
      this.categoryService.deleteCategory(this.categoryToDelete).subscribe({
        next: () => {
          this.categories = this.categories.filter(c => c.id !== this.categoryToDelete);
          this.toastr.success('Category deleted successfully!', 'Deleted');
          this.showConfirmDelete = false;
          this.categoryToDelete = null;
        }
      });
    }
  }
}
