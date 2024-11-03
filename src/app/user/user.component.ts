import { Component } from '@angular/core';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';



@Component({
  selector: 'app-user',
  standalone: true,
  imports: [RegistrationComponent, LoginComponent, CommonModule,RouterOutlet],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent {
  showRegistration: boolean = true;

  switchToRegistration() {
    this.showRegistration = true;
  }

  switchToLogin() {
    this.showRegistration = false;
  }
}
