import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
@Component({
  selector: 'app-left-sidebar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './left-sidebar.component.html',
  styleUrl: './left-sidebar.component.css',
})
export class LeftSidebarComponent {
  isLeftSidebarCollapsed = input.required<boolean>();
  changeIsLeftSidebarCollapsed = output<boolean>();
  items: any[] = [];
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.updateSidebarItems();
  }

  updateSidebarItems(): void {
    const user = this.authService.getCustomer();
    if (user && user.id == 20) {
      this.items = [
        {
          routeLink: 'home/home-admin',
          icon: 'fal fa-home',
          label: 'Dashboard',
        },
        {
          routeLink: 'home/car-parts',
          icon: 'fal fa-car-battery',
          label: 'Car Parts',
        },
        {
          routeLink: 'home/home-mechanic',
          icon: 'fal fa-solid fa-users',
          label: 'Mechanics',
        },
        {
          routeLink: 'home/statistics',
          icon: 'fal fa-chart-line',
          label: 'Statistics',
        },
        {
          routeLink: 'login',
          icon: 'fal fa-solid fa-user',
          label: 'Log Out',
        }
      ];
    } else {
      this.items = [
        {
          routeLink: 'home/main',
          icon: 'fal fa-home',
          label: 'Home',
        },
        {
          routeLink: 'home/my-car',
          icon: 'fal fa-solid fa-car',
          label: 'My Car',
        },
        {
          routeLink: 'request-service',
          icon: 'fas fa-toolbox',
          label: 'Request Service',
        },
        {
          routeLink: 'home/car-parts',
          icon: 'fal fa-car-battery',
          label: 'Car Parts',
        },
        {
          routeLink: 'login',
          icon: 'fal fa-solid fa-user',
          label: 'Login',
        }
      ];
    }
  }

  

  toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed());
  }

  closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }
}