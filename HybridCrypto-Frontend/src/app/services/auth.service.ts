import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loginURL = environment.apiURL + 'Authentication/token/';
  private registerURL = environment.apiURL + 'Authentication/register/';
  private twoFactorURL = environment.apiURL + 'Authentication/2FA/';
  private loggedIn = new BehaviorSubject<boolean>(this.hasValidToken());

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelperService: JwtHelperService
  ) {}

  register(data): any {
    return this.http.post(this.registerURL, data);
  }

  login(data): any {
    localStorage.clear();
    return this.http.post(this.loginURL, data);
  }

  submitTwoFactorCode(data): any {
    return this.http.post(this.twoFactorURL, data);
  }

  logout() {
    localStorage.clear();
    this.updateAuthentication();
    this.router.navigate(['/login']);
  }

  //#region Token management

  saveToken(token) {
    localStorage.setItem('token', token);
    this.loggedIn.next(true);
    this.router.navigate(['/inbox']);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  updateAuthentication(): void {
    this.loggedIn.next(this.hasValidToken());
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  hasValidToken(): boolean {
    const token = this.getToken();
    try {
      return !this.jwtHelperService.isTokenExpired(token);
    } catch {
      return false;
    }
  }

  //#endregion
}
