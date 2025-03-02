import { Routes, RouterModule} from '@angular/router';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { HomeComponent } from './home/home.component';
import { FooterComponent } from './footer/footer.component';
import { MainComponent } from './main/main.component';
import { MyCarComponent } from './my-car/my-car.component';
import { AuthGuard } from './shared/auth.guard';
import { RequestServiceComponent } from './request-service/request-service.component';
import { CarPartsComponent } from './car-parts/car-parts.component';
import { HomeAdminComponent } from './home-admin/home-admin.component';
import { AdminGuard } from './shared/admin.guard';
import { MechanicsComponent } from './mechanics/mechanics.component';
import { StatisticsAdminComponent } from './statistics-admin/statistics-admin.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
  { path : 'request-service', component: RequestServiceComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegistrationComponent },
  { path : 'home/main', component: MainComponent, canActivate: [AuthGuard] },
  { path : 'home/my-car', component: MyCarComponent, canActivate: [AuthGuard] },
  { path : 'home/car-parts', component: CarPartsComponent, canActivate: [AuthGuard] },
  { path: 'home/home-admin', component: HomeAdminComponent, canActivate: [AdminGuard] },
  {path : 'home/home-mechanic', component: MechanicsComponent, canActivate: [AdminGuard]},
  {path : 'home/statistics', component: StatisticsAdminComponent, canActivate: [AdminGuard]},
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];