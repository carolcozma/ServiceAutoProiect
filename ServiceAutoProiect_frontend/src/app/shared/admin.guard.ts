import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private service: AuthService, private router: Router) {}

  canActivate(): boolean {
    const user = this.service.getCustomer();
    console.log('AdminGuard: Checking if user is admin', user);
    if (user && user.id == 20) { // Adjust the condition based on your admin identification logic
      console.log('AdminGuard: User is admin');
      return true;
    } else {
      console.log('AdminGuard: User is not admin, redirecting to login');
      this.router.navigate(['/login']);
      return false;
    }
  }
}