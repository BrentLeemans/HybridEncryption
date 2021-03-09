import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
})
export class AppComponent {
  isAuthenticated: boolean;

  constructor(private authService: AuthService) {
    this.authService
      .isLoggedIn()
      .subscribe((bool) => (this.isAuthenticated = bool));
  }
}
