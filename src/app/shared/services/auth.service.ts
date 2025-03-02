// customer.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../../models/customer.model';
import { CommonModule } from '@angular/common';
import { Car } from '../../models/car.model';
import { Service, ServiceUpdate } from '../../models/service.model';
import { ServiceType } from '../../models/servicetype.model';
import { CarPart } from '../../models/carpart.model';
import { Part } from '../../models/service.model';
import { PendingService,CarPartAdd } from '../../models/service.model';
import { ServicePart } from '../../models/service.model';
import { Mechanic } from '../../models/service.model';
@Injectable({
  providedIn: 'root'
})



export class AuthService {
  private apiUrl = 'https://localhost:44321/api/Customers'; 
  private customer: any;
  private car: any;
  private service: any;

  constructor(private http: HttpClient) { }

  addCustomer(customer: Customer): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/AddCustomer`, customer, { headers });
  }
  getCustomerById(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/GetCustomer/${id}`);
  }
  Login(email: string, password: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { email, password };
    return this.http.post(`${this.apiUrl}/Login`, body, { headers });
  }
  getCustomerByEmail(email: string): Observable<any> {
    const params = new HttpParams().set('email', email);
    return this.http.get(`${this.apiUrl}/GetCustomerByEmail`, { params });
  }

  setCustomer(customer: any): void {
    this.customer = customer;
    if (typeof window !== 'undefined' && window.localStorage) {
      localStorage.setItem('customer', JSON.stringify(customer));
    }
  }

  getCustomer(): any {
    if (!this.customer && typeof window !== 'undefined' && window.localStorage) {
      this.customer = JSON.parse(localStorage.getItem('customer') || '{}');
    }
    return this.customer;
  }

  addCar(car: Car): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/AddCar`, car, { headers });

  }

  loadCars(customerID: number): Observable<Car[]> {
    const params = new HttpParams().set('customerID', customerID);
    return this.http.get<Car[]>(`${this.apiUrl}/GetCarsByCustomerID`, { params });
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('customer');
  }

  getServiceHistory(carId: number): Observable<Service[]> {
    const params = new HttpParams().set('CarID', carId);
    return this.http.get<Service[]>(`${this.apiUrl}/GetServicesByCarId`, { params });
  }

  getServiceTypes(): Observable<ServiceType[]> {
    return this.http.get<ServiceType[]>(`${this.apiUrl}/GetServiceTypes`);
  }

  addService(carId: number, serviceTypeId: number): Observable<any> {
    const url = `${this.apiUrl}/AddService`;
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { carId, serviceTypeId };
  
    return this.http.post<any>(url, body, { headers });
  }

  getCarParts(): Observable<CarPart[]> {
    return this.http.get<CarPart[]>(`${this.apiUrl}/GetCarParts`);
  }

  getPartsForService(serviceID: number): Observable<Part[]> {
    return this.http.get<Part[]>(`${this.apiUrl}/GetServicePartsByServiceId?serviceId=${serviceID}`);
  }

  deleteCar(carID: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteCar?carID=${carID}`);
  }

  deleteService(serviceID: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteService?serviceID=${serviceID}`);
  }

  isAdmin(): boolean {
    const user = this.getCustomer();
    return user && user.id == 20; // Adjust the condition based on your admin identification logic
  }

  getAllServices(): Observable<PendingService[]> {
    return this.http.get<PendingService[]>(`${this.apiUrl}/GetAllServices`);
  }

  updateService(serviceID: number, service: ServiceUpdate): Observable<any> {
    const url = `${this.apiUrl}/UpdateService/${serviceID}`;
    return this.http.put<any>(url, service, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }
  getMechanics(): Observable<Mechanic[]> {
    return this.http.get<Mechanic[]>(`${this.apiUrl}/GetAllMechanics`);
  }

  
  addCarPartToService(serviceID: number, carPartId: number, quantity: number): Observable<any> {
    const body = { carPartId, quantity };
    return this.http.post<CarPartAdd>(`${this.apiUrl}/AddCarPartToService/${serviceID}/${carPartId}/${quantity}`, body);
  }

  deleteCarPartFromService(serviceID: number, partID: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteCarPartFromService/${serviceID}/${partID}`);
  }

  getPartsAllForService(serviceID: number): Observable<ServicePart[]> {
    return this.http.get<ServicePart[]>(`${this.apiUrl}/GetServicePartsAllByServiceId?serviceId=${serviceID}`);
  }
  getAllCarParts(): Observable<CarPartAdd[]> {
    return this.http.get<CarPartAdd[]>(`${this.apiUrl}/GetAllCarParts`);
  }
  getServicesByMechanicId(mechanicId: number): Observable<PendingService[]> {
    return this.http.get<PendingService[]>(`${this.apiUrl}/GetServicesByMechanicId/${mechanicId}`);
  }

  getTotalServices(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/statistics/total-services`);
  }

  getTotalRevenue(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/statistics/total-revenue`);
  }

  getAverageServiceCost(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/statistics/average-service-cost`);
  }

  getTopCustomers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/statistics/top-customers`);
  }

  getMechanicPerformance(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/statistics/mechanic-performance`);
  }

  getLowStockParts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/statistics/low-stock-parts`);
  }

  getServiceTypeAnalysis(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/statistics/service-type-analysis`);
  }

  updateCar(car: any): Observable<any> {
    console.log('Car:', car); // Print the updated car object

    return this.http.put<any>(`${this.apiUrl}/UpdateCar`, car);
  }

  getServiceTypeDistribution(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/service-type-distribution`);
  }

  getTopMechanicsByRevenue(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/top-mechanics-by-revenue`);
  }

  getMonthlyRevenueAndServiceCount(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/monthly-revenue-and-service-count`);
  }
}