import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouterModule, Routes } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { routes } from './app.routes';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,
    FooterComponent,
    UserComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',

})
export class AppComponent {
  title = 'ServiceAutoProiect';
}

