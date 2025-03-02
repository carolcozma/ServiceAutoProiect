import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { PendingService, CarPartAdd } from '../models/service.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ServicePart } from '../models/service.model';
import {ServiceUpdate} from '../models/service.model';
import { Mechanic } from '../models/service.model';
import { Router } from '@angular/router';
@Component({
  selector: 'app-home-admin',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home-admin.component.html',
  styleUrl: './home-admin.component.css'
})
export class HomeAdminComponent implements OnInit {
  allServices: PendingService[] = [];
  displayedServices: PendingService[] = [];
  currentPage: number = 1;
  itemsPerPage: number = 10;
  editingService: ServiceUpdate | null = null;
  carParts: CarPartAdd[] = [];
  filteredCarParts: CarPartAdd[] = [];
  newCarPart: CarPartAdd = { carPartId: 0, partName: '', partPrice: 0, partQuantity: 0 };
  searchQuery: string = '';
  serviceCarParts:ServicePart[] = [];
  selectedCarPart: CarPartAdd | null = null; 
  mechanics: Mechanic[] = [];


  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    
    this.loadAllServices();
    this.loadCarParts();
  }

  loadAllServices(): void {
    this.authService.getAllServices().subscribe(data => {
      this.allServices = data;
      this.updateDisplayedServices();
      console.log(this.allServices); // Log the latest pending services to the console
    });
  }

  loadCarParts(): void {
    this.authService.getAllCarParts().subscribe(data => {
      this.carParts = data;
      this.filteredCarParts = data;
    });
  }

  
  loadMechanics(): void {
    this.authService.getMechanics().subscribe(data => {
      this.mechanics = data;
      console.log('Loaded mechanics:', this.mechanics);
    });
  }

  updateDisplayedServices(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.displayedServices = this.allServices.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage = page;
      this.loadAllServices();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages()) {
      this.currentPage++;
      this.loadAllServices();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadAllServices();
    }
  }

  totalPages(): number {
    return Math.ceil(this.allServices.length / this.itemsPerPage);
  }

  deleteService(serviceID: number): void {
    if (confirm('Are you sure you want to delete this service?')) {
      this.authService.deleteService(serviceID).subscribe(() => {
        this.allServices = this.allServices.filter(service => service.serviceID !== serviceID);
        this.loadAllServices();
      });
    }
  }

  editService(service: PendingService): void {
    this.loadMechanics();
    this.editingService = {
      serviceId: service.serviceID,
      carId: service.carId,
      serviceTypeId: service.serviceTypeId,
      mechanicId: service.mechanicId,
      serviceDate: service.serviceDate,
      description: service.description || "",
      totalCost: service.totalCost || 0,
      status: service.status
    };
    this.loadCarPartsForService(service.serviceID);
  }

  saveService(): void {
    console.log(this.editingService);
    console.log(this.mechanics);
    if (this.editingService) {
      this.authService.updateService(this.editingService.serviceId, this.editingService).subscribe(updatedService => {
        const index = this.allServices.findIndex(service => service.serviceID === updatedService.serviceID);
        if (index !== -1) {
          this.allServices[index] = updatedService;
          this.loadAllServices();
        }
        this.editingService = null;
      });
    }
    location.reload();
  }

  cancelEdit(): void {
    this.editingService = null;
    this.serviceCarParts = [];
  }

  loadCarPartsForService(serviceID: number): void {
    this.authService.getPartsAllForService(serviceID).subscribe(data => {
      console.log(data);
      this.serviceCarParts = data;
    });
  }

  addCarPart(): void {

    if (this.newCarPart.carPartId && this.editingService) {
      this.authService.addCarPartToService(this.editingService.serviceId, this.newCarPart.carPartId, this.newCarPart.partQuantity).subscribe(addedPart => {
        if (this.editingService) {
          this.loadCarPartsForService(this.editingService.serviceId);
        }
        this.newCarPart = { carPartId: 0, partName: '', partPrice: 0, partQuantity: 0 };
        this.selectedCarPart = null;
      });
    }
  }

  deleteCarPart(partID: number): void {
    if (this.editingService) {
      console.log(this.serviceCarParts);
      this.authService.deleteCarPartFromService(this.editingService.serviceId, partID).subscribe(() => {
        if (this.editingService) {
          this.loadCarPartsForService(this.editingService.serviceId);
        }

      });
    }
  }

}