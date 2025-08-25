import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { Observable } from 'rxjs';
import { CreateWalletDto, WalletDto, WalletWithExpensesDto } from '../model/WalletDto';

@Injectable({ providedIn: 'root' })
export class WalletService {
  private apiUrl = `${environment.apiUrl}/wallet`;

  constructor(
    private http: HttpClient,
) {}

  getAllWallet(): Observable<WalletDto[]> {
    return this.http.get<WalletDto[]>(this.apiUrl);
  }

  getWalletById(id: number): Observable<WalletDto> {
    return this.http.get<WalletDto>(`${this.apiUrl}/${id}`);
  } 

  getWalletWithExpensesById(id: number): Observable<WalletWithExpensesDto> {
  return this.http.get<WalletWithExpensesDto>(`${this.apiUrl}/${id}/details`);
  }

  getLatestWallet(): Observable<WalletWithExpensesDto> {
    return this.http.get<WalletWithExpensesDto>(`${this.apiUrl}/latest`);
  }

  createWallet(dto: CreateWalletDto): Observable<WalletDto> {
    return this.http.post<WalletDto>(this.apiUrl, dto);
  }

  deleteWallet(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

   archiveWallet(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/archive`, {});
  }

  getArchivedWallets(): Observable<WalletDto[]> {
    return this.http.get<WalletDto[]>(`${this.apiUrl}/archived`);
  }
}
