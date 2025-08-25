import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from '../model/CategoryDto ';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private apiUrl = `${environment.apiUrl}/category`;

  constructor(
    private http: HttpClient,
) {}

  getAllCategory(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(this.apiUrl);
  }

  getCategoryById(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.apiUrl}/${id}`);
  } 

  createCategory(dto: CreateCategoryDto): Observable<CategoryDto> {
    return this.http.post<CategoryDto>(this.apiUrl, dto);
  }

  updateCategory(id: number, dto: UpdateCategoryDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
