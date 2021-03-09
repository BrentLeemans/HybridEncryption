import {Component, OnInit} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import {AuthService} from '../services/auth.service';
import {Router} from '@angular/router';
import {TwoFactorImage} from '../models/two-factor-image';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  display = false;
  errors: any;
  twoFactorData: TwoFactorImage;

  constructor(
    private fg: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = new FormGroup({
      nickname: new FormControl('', [
        Validators.required,
        Validators.pattern('^[A-Za-z ]+$'),
      ]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [
        Validators.required,
        // Can't enter a space
        Validators.pattern(
          '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&`~*()_=+,<.>/?{};:\'\\- \\[\\]"]).{8,}'
        ),
      ]),
    });
  }

  ngOnInit(): void {
  }

  register() {

    this.authService.register(this.registerForm.value).subscribe(
      (twoFactorData) => {
        this.twoFactorData = twoFactorData;
        this.display = true;
      },
      (error) => {
        this.errors = Object.values(error.error);
      }
    );
  }

  // Getters used for validation
  get nickname() {
    return this.registerForm.get('nickname');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }
}
