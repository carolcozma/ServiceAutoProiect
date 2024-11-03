import { Component } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  EmailValidator,
  Validators 
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../shared/services/auth.service';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  form: any;
  isSubmitted: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';


  constructor(public formBuilder: FormBuilder, private service: AuthService, private router: Router) {
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
      
  onSubmit() {
    this.isSubmitted = true;
    this.errorMessage = '';
    this.successMessage = '';
    if (this.form.valid) {
      this.service.Login(this.form.value.email, this.form.value.password).subscribe(
        response => {
          this.successMessage = 'Login successfull';
        },
        error => {
            this.errorMessage = 'Incorrect credentials';
        }
      );
    } else {
      this.errorMessage = 'Form is invalid';
    }
  }
}
