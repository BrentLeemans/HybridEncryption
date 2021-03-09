import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  twoFactorForm: FormGroup;
  wrongCredentials: boolean;
  errorMessage: string;
  displayGoogleAuthInput = false;
  wrongAuthenticatorCodeErrorMessage: string;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });
  }

  ngOnInit(): void {}

  login() {
    this.authService.login(this.loginForm.value).subscribe(
      (data) => {
        this.displayGoogleAuthInput = true;
        this.twoFactorForm = new FormGroup({
          email: new FormControl(this.loginForm.value.email, [
            Validators.email,
          ]),
          code: new FormControl('', [
            Validators.minLength(6),
            Validators.maxLength(6),
          ]),
          guid: new FormControl(data.guid),
        });
      },

      (error) => {
        console.log(error);
        if (error.status === 403) {
          this.wrongCredentials = true;
          this.errorMessage = error.error;
        }
      }
    );
  }

  submitCode() {
    console.log(this.twoFactorForm.value);
    if (isNaN(this.twoFactorForm.get('code').value)) {
      this.wrongAuthenticatorCodeErrorMessage = 'Please enter a number!';
      return;
    }

    this.authService.submitTwoFactorCode(this.twoFactorForm.value).subscribe(
      (token) => this.authService.saveToken(token),
      (error) => {
        if (error.status === 403) {
          this.wrongAuthenticatorCodeErrorMessage = error.error;
        }
      }
    );
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}
