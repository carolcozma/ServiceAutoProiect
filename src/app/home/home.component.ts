import { Component,  HostListener,computed, Input,signal  } from '@angular/core';
import { LeftSidebarComponent } from '../left-sidebar/left-sidebar.component';
import { Router, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../shared/services/auth.service';



@Component({
  selector: 'app-home',
  standalone: true,
  imports: [LeftSidebarComponent,RouterOutlet, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Input() isLeftSidebarCollapsed: boolean = false; // Initialize with default value
  @Input() screenWidth: number = 0; // Initialize with default value

  constructor(private service: AuthService, private router: Router) {}


  ngOnInit(): void {
    if (this.router.url === '/login' || this.router.url === '/registration') {
      this.router.navigate(['login']);
    }
  }

  sizeClass() {
    if (this.isLeftSidebarCollapsed) {
      return '';
    }
    return this.screenWidth > 768 ? 'body-trimmed' : 'body-md-screen';
  }

}
