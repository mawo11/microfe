import { Component, effect, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from '../models/auth.model';
import { SessionStore } from '../store/session';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  submitted = false;
  
  errorMessage: string | null = null;
  session = inject(SessionStore);
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,

  ) { 
    this.session.clear();
    this.session.checkSession();
    
    effect(() => {
      if (this.session.isLoggedIn()) {
        const url = this.session.getStartPageUrl();
        console.log('Redirecting to:', url);
       document.location.href = this.session.getStartPageUrl() || '';
      }
    });

  }

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = null;

    if (this.loginForm.invalid) {
      return;
    }

    const credentials: LoginRequest = this.loginForm.value;
    this.session.login(credentials.email, credentials.password);
  }

  get isFormInvalid(): boolean {
    return this.submitted && this.loginForm.invalid;
  }
}
