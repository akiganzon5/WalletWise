import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { LoginDto, RegisterDto } from '../../model/RegisterDto';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-auth-landing',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './auth-landing.component.html',
  styleUrl: './auth-landing.component.css'
})
export class AuthLandingComponent {
  isLogin = true;
  loginDto: LoginDto = { email: '', password: '' };
  registerDto: RegisterDto = { username: '', email: '', password: '' };
  loading = false;
  error = '';

  constructor(
    private auth: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  toggleMode() { this.isLogin = !this.isLogin; this.error = ''; }

  onSubmit() {
    if (this.loading) return;
    this.loading = true;
    this.error = '';

    if (this.isLogin) {
      this.auth.login(this.loginDto).subscribe({
        next: res => { 
          this.auth.saveToken(res.token); 
          this.toastr.success('Login successful!', 'Welcome 🎉');
          this.router.navigate(['/wallet']);  
        },
        error: err => { 
          this.toastr.error('Invalid email or password', 'Login Failed');
          console.error(err); 
          this.loading = false; 
        },
        complete: () => this.loading = false
      });
    } else {
      this.auth.register(this.registerDto).subscribe({
        next: () => { 
          this.isLogin = true; 
          this.toastr.success('Account created! Please login.', 'Success ✅');
        },
        error: err => { 
          this.toastr.error('Something went wrong. Try again later.', 'Registration Failed');
          console.error(err); 
          this.loading = false; 
        },
        complete: () => this.loading = false
      });
    }
  }
}
