import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PendingService, Mechanic } from '../models/service.model';
import { AuthService } from '../shared/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mechanics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mechanics.component.html',
  styleUrls: ['./mechanics.component.css']
})
export class MechanicsComponent implements OnInit {
  services: PendingService[] = [];
  mechanics: Mechanic[] = [];
  selectedMechanicId: number | null = null;
  noServicesMessage: string = '';

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.getMechanics().subscribe((data: Mechanic[]) => {
      this.mechanics = data;
    });
  }

  onMechanicChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const mechanicId = selectElement.value;
    const id = Number(mechanicId);
    if (!isNaN(id)) {
      this.selectedMechanicId = id;
      this.authService.getServicesByMechanicId(id).subscribe((data: PendingService[]) => {
        this.services = data;
        this.noServicesMessage = this.services.length === 0 ? 'No services found for the selected mechanic.' : '';
      });
    }
  }
}