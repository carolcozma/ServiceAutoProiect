import { Component, HostListener, OnInit, signal } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { LeftSidebarComponent } from './left-sidebar/left-sidebar.component';
import { HomeComponent } from './home/home.component';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { CommonModule } from '@angular/common';
import { AuthService } from './shared/services/auth.service';
import { FormsModule } from '@angular/forms'; // Import FormsModule


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    FooterComponent,
    LeftSidebarComponent,
    HomeComponent,
    LoginComponent,
    RegistrationComponent,
    CommonModule,
    FormsModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'ServiceAutoProiect';
  isLeftSidebarCollapsed = signal<boolean>(false);
  screenWidth = signal<number>(0);
  showSidebar = signal<boolean>(true);
  currentRoute = signal<string>('');

  constructor(private router: Router, private route: ActivatedRoute, private service: AuthService) {
    if (typeof window !== 'undefined') {
      this.screenWidth.set(window.innerWidth);
    }

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      const currentRoute = this.router.url;
      this.currentRoute.set(currentRoute);
      this.showSidebar.set(currentRoute !== '/login' && currentRoute !== '/registration');
    });
  }

  @HostListener('window:resize')
  onResize() {
    if (typeof window !== 'undefined') {
      this.screenWidth.set(window.innerWidth);
      if (this.screenWidth() < 768) {
        this.isLeftSidebarCollapsed.set(true);
      }
    }
  }

  ngOnInit(): void {
    if (typeof window !== 'undefined') {
      this.isLeftSidebarCollapsed.set(this.screenWidth() < 768);
    }
  }

  changeIsLeftSidebarCollapsed(isLeftSidebarCollapsed: boolean): void {
    this.isLeftSidebarCollapsed.set(isLeftSidebarCollapsed);
  }

  isLoginRoute(): boolean {
    return this.currentRoute() === '/login';
  }

  isRegistrationRoute(): boolean {
    return this.router.url === '/registration';
  }

  isAuthenticated(): boolean {
    return this.service.isAuthenticated();
  }
}