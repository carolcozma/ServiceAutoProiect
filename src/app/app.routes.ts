import { Routes, RouterModule} from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path: 'user', component: UserComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];