// customer.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../../models/customer.model';


@Injectable({
  providedIn: 'root'
})



export class AuthService {
  private apiUrl = 'https://localhost:44321/api/Customers'; 

  constructor(private http: HttpClient) { }

  addCustomer(customer: Customer): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/AddCustomer`, customer, { headers });
  }
  getCustomer(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/GetCustomer/${id}`);
  }
  Login(email: string, password: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { email, password };
    return this.http.post(`${this.apiUrl}/Login`, body, { headers });
  }
}