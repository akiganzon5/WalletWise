import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.prod';
import { RegisterDto, LoginDto, AuthResponseDto } from '../model/RegisterDto';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router'; 


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  login(dto: LoginDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.apiUrl}/login`, dto);
  }

  register(dto: RegisterDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/register`, dto);
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    if (typeof window !== 'undefined' && typeof localStorage !== 'undefined') {
      return localStorage.getItem('token');
    }
    return null;
  }


  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/']); 
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
      if (!token) return false;

      try {
        const decoded: any = jwtDecode(token);
        const now = Date.now().valueOf() / 1000;
        return decoded.exp && decoded.exp > now; 
      } catch (e) {
        return false;
      }
  }
}
