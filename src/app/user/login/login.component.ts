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
import { Customer } from '../../models/customer.model';


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
  customer: any;


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
          this.service.getCustomerByEmail(this.form.value.email).subscribe(
            data => {
              this.customer = data;
              this.service.setCustomer(this.customer);
              console.log(this.customer);
              this.router.navigate(['/home/main']);
            },
            error => {
              console.error('Error fetching customer data', error);
            }
          );
          
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
