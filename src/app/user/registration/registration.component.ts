import { Component, ViewChild } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  EmailValidator,
  Validators 
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../shared/services/auth.service';
import { Router } from '@angular/router';
import { UserComponent } from '../user.component';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, UserComponent,RouterModule],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css',
})
export class RegistrationComponent {
  @ViewChild(UserComponent) userComponent!: UserComponent;
  form: any;
  isSubmitted: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';


  constructor(public formBuilder: FormBuilder, private service: AuthService, private router: Router) {
    this.form = this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      address: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.errorMessage = '';
    this.successMessage = '';
    if (this.form.valid) {
      this.service.addCustomer(this.form.value).subscribe(
        response => {
          this.successMessage = 'Account created successfully';
            this.router.navigate(['/login']);
        },
        error => {
          if (error.status === 409) {
            this.errorMessage = 'User already exists';
          } else {
            this.errorMessage = 'Error adding user';
          }
        }
      );
    } else {
      this.errorMessage = 'Form is invalid';
    }
  }
  
  }

