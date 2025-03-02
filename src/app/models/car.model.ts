import { Service } from './service.model';

export interface Car {
  carID: number;
  customerID: number;
  make: string;
  model: string;
  year: number;
  vin: string;
  licensePlate: string;
  isExpanded?: boolean; // Optional property to track expanded state
  serviceHistory?: Service[]; // Optional property for service history
}

