import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { Observable } from 'rxjs';
import { CreateExpenseDto, ExpenseDto, UpdateExpenseDto } from '../model/ExpenseDto';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
  private apiUrl = `${environment.apiUrl}/expense`;

  constructor(
    private http: HttpClient,
) {}

  getAllExpenses(): Observable<ExpenseDto[]> {
    return this.http.get<ExpenseDto[]>(this.apiUrl);
  }

  getExpenseById(id: number): Observable<ExpenseDto> {
    return this.http.get<ExpenseDto>(`${this.apiUrl}/${id}`);
  } 

  createExpense(dto: CreateExpenseDto): Observable<ExpenseDto> {
    return this.http.post<ExpenseDto>(this.apiUrl, dto);
  }

  updateExpense(id: number, dto: UpdateExpenseDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
