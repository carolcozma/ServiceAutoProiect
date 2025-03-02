import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { CarPart } from './../models/carpart.model';
import { AuthService } from './../shared/services/auth.service';
import { FormsModule } from '@angular/forms'; // Import FormsModule


@Component({
  selector: 'app-car-parts',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './car-parts.component.html',
  styleUrl: './car-parts.component.css'
})
export class CarPartsComponent {
  carParts: CarPart[] = [];
  filteredCarParts: CarPart[] = [];
  searchQuery: string = '';
  sortCriteria: string = '';

  constructor(private service: AuthService) {}

  ngOnInit(): void {
    this.fetchCarParts();
    
  }

  fetchCarParts(): void {
    this.service.getCarParts().subscribe(data => {
      this.carParts = data;
      this.filteredCarParts = data;
      console.log(this.carParts); // Log the car parts to the console
    }, error => {
      console.error('Error fetching car parts:', error);
    });
  }

  filterAndSortParts(): void {
    let parts = this.carParts;

    // Filter parts based on search query
    if (this.searchQuery) {
      parts = parts.filter(part =>
        part.partName.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }

    // Sort parts based on selected criteria
    if (this.sortCriteria) {
      parts = parts.sort((a, b) => {
        if (this.sortCriteria === 'name') {
          return a.partName.localeCompare(b.partName);
        } else if (this.sortCriteria === 'price') {
          return a.price - b.price;
        } else if (this.sortCriteria === 'quantity') {
          return a.quantity - b.quantity;
        }
        return 0;
      });
    }

    this.filteredCarParts = parts;
  }
}
