export interface Part {
  partName: string;
  partPrice: number;
  partQuantity: number;
}

export interface Service {
  serviceID: number;
  serviceName: string;
  description: string;
  totalCost: number;
  serviceDate: Date;
  status: string;
  mechanicFirstName: string;
  mechanicLastName: string;
  parts: Part[];
}

export interface PendingService {
  serviceID: number;
  carMake: string;
  carModel: string;
  customerFirstName: string;
  customerLastName: string;
  serviceDate: Date;
  status: string;
  totalCost: number;
  description: string;
  mechanicId: number;
  serviceTypeId: number;
  carId: number;
}

export interface CarPartAdd {
  carPartId: number;
  partName: string;
  partPrice: number;
  partQuantity: number;
}

export interface ServicePart {
  partId: number;
  partName: string;
  price: number;
  quantity: number;

}

export interface ServiceUpdate {
  serviceId: number;
  carId: number;
  serviceTypeId: number;
  mechanicId: number;
  serviceDate: Date;
  description: string;
  totalCost: number;
  status: string;
}

export interface Mechanic {
  mechanicId: number;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
}