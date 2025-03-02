import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  firstName: string = '';
  customer: any;

  constructor(private service: AuthService) {}

  ngOnInit(): void {
    this.customer = this.service.getCustomer();

  }
}
