import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService {
  private isAuthenticated: boolean;
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    this.authService
      .isLoggedIn()
      .subscribe((bool) => (this.isAuthenticated = bool));
    if (this.isAuthenticated) {
      return true;
    } else {
      this.router.navigate(['/logout']);
      return false;
    }
  }
}
