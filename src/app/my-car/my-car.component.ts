import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Car } from '../models/car.model';
import { AuthService } from '../shared/services/auth.service';
import { Service } from '../models/service.model';
import { Part } from '../models/service.model';

@Component({
  selector: 'app-my-car',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './my-car.component.html',
  styleUrls: ['./my-car.component.css']
})
export class MyCarComponent implements OnInit {
  cars: Car[] = [];
  carForm: FormGroup;
  editCarForm: FormGroup;
  selectedCar: Car | null = null;
  isEditModalOpen = false;

  constructor(private formBuilder: FormBuilder, private service: AuthService) {
    this.carForm = this.formBuilder.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', [Validators.required, Validators.min(1886), Validators.max(new Date().getFullYear())]],
      vin: ['', [Validators.required, Validators.maxLength(17)]],
      licensePlate: ['', [Validators.required]]
    });

    this.editCarForm = this.formBuilder.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', Validators.required],
      vin: ['', Validators.required],
      licensePlate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadCars();
  }

  loadCars(): void {
    const customer = this.service.getCustomer();
    if (customer && customer.id) {
      const customerId = customer.id.toString(); // Ensure customerId is a string
      this.service.loadCars(customerId).subscribe(
        (data: Car[]) => {
          this.cars = data;
          console.log(this.cars);
        },
        (error) => {
          console.log(error);
        }
      );
    } else {
      console.error('Customer ID is undefined');
    }
  }

  onEditSubmit(): void {
    if (this.editCarForm.valid && this.selectedCar) {
      const updatedCar = { ...this.selectedCar, ...this.editCarForm.value };
      console.log('Updated Car:', updatedCar); // Print the updated car object
      this.service.updateCar(updatedCar).subscribe(() => {
        this.loadCars();
        this.editCarForm.reset();
        this.closeEditModal();
      });
    }
  }

  editCar(car: Car): void {
    this.selectedCar = car;
    this.editCarForm.patchValue(car);
    this.isEditModalOpen = true;
  }

  onSubmit() {
    if (this.carForm.valid) {
      const newCar: Car = {
        ...this.carForm.value,
        year: this.carForm.value.year.toString(), // Convert year to string
        customerID: this.service.getCustomer().id
      };

      this.service.addCar(newCar).subscribe(
        (data) => {
          console.log('Car added successfully');
          this.loadCars();
        },
        (error) => {
          console.log(error);
          console.log(newCar);
        }
      );
    } else {
      console.log('Form is invalid');
    }
  }

  toggleExpand(car: Car): void {
    car.isExpanded = !car.isExpanded;
    if (car.isExpanded && !car.serviceHistory) {
      this.loadServiceHistory(car);
    }
  }

  loadServiceHistory(car: Car): void {
    this.service.getServiceHistory(car.carID).subscribe(
      (data: Service[]) => {
        car.serviceHistory = data;
        car.serviceHistory.forEach(service => {
          this.loadPartsForService(service);
        });
        console.log(car.serviceHistory); // Log the service history to the console
      },
      (error) => {
        console.log(error);
      }
    );
  }

  loadPartsForService(service: Service): void {
    this.service.getPartsForService(service.serviceID).subscribe(
      (data: Part[]) => {
        service.parts = data;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  deleteCar(car: Car): void {
    if (car.carID && confirm(`Are you sure you want to delete the car ${car.make} ${car.model}?`)) {
      this.service.deleteCar(car.carID).subscribe(
        () => {
          this.cars = this.cars.filter(c => c.carID !== car.carID);
          console.log(`Car ${car.make} ${car.model} deleted successfully`);
        },
        (error) => {
          console.log(error);
        }
      );
    } else {
      console.error('Car ID is undefined');
    }
  }

  deleteService(car: Car, service: Service): void {
    if (service.serviceID && confirm(`Are you sure you want to delete the service on ${service.serviceDate}?`)) {
      this.service.deleteService(service.serviceID).subscribe(
        () => {
          if (car.serviceHistory) {
            car.serviceHistory = car.serviceHistory.filter(s => s.serviceID !== service.serviceID);
          }
          console.log(`Service on ${service.serviceDate} deleted successfully`);
        },
        (error) => {
          console.log(error);
        }
      );
    } else {
      console.error('Service ID is undefined');
    }
  }

  closeEditModal(): void {
    this.isEditModalOpen = false;
  }
}