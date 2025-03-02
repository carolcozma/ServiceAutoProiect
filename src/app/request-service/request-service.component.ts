import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Car} from '../models/car.model';
import { AuthService } from '../shared/services/auth.service';
import { ServiceType } from '../models/servicetype.model';

@Component({
  selector: 'app-request-service',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './request-service.component.html',
  styleUrls: ['./request-service.component.css']
})
export class RequestServiceComponent implements OnInit {
  requestServiceForm: FormGroup;
  cars: Car[] = [];
  serviceTypes: ServiceType[] = [];
  successMessage: string | null = null;

  constructor(private formBuilder: FormBuilder, private service: AuthService) {
    this.requestServiceForm = this.formBuilder.group({
      car: ['', Validators.required],
      service: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    const customer = this.service.getCustomer();
    if (customer && customer.id) {
      const customerId = customer.id.toString(); // Ensure customerId is a string
      this.service.loadCars(customerId).subscribe(
        (data: Car[]) => {
          this.cars = data;
          console.log('Cars loaded:', this.cars); // Debugging log
        },
        (error) => {
          console.log(error);
        }
      );
    } else {
      console.error('Customer ID is undefined');
    }

    this.service.getServiceTypes().subscribe(
      (data: ServiceType[]) => {
        this.serviceTypes = data;
        console.log('Service Types loaded:', this.serviceTypes); // Debugging log
      },
      (error) => {
        console.log(error);
      }
    );
  }

  onSubmit() {
    if (this.requestServiceForm.valid) {
      const requestData = this.requestServiceForm.value;
      this.requestService(requestData.car, requestData.service);
    } else {
      console.log('Form is invalid');
    }
  }

  requestService(carId: number, serviceTypeId: number): void {
    this.service.addService(carId, serviceTypeId).subscribe(
      (data) => {
        console.log('Service requested successfully');
        this.successMessage = 'Request sent successfully!';
        this.requestServiceForm.reset();
      },
      (error) => {
        console.log('Service request submitted:', carId, serviceTypeId);
        console.log(error);
      }
    );
  }
}