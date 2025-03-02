import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-statistics-admin',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './statistics-admin.component.html',
  styleUrl: './statistics-admin.component.css'
})
export class StatisticsAdminComponent implements OnInit {
  totalServices: number = 0;
  totalRevenue: number = 0;
  averageServiceCost: number = 0;
  topCustomers: any[] = [];
  mechanicPerformance: any[] = [];
  lowStockParts: any[] = [];
  serviceTypeAnalysis: any[] = [];
  topMechanicsByRevenue: any[] = [];
  monthlyRevenueAndServiceCount: any[] = [];

  constructor(private service: AuthService) {}

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.service.getTotalServices().subscribe(data => this.totalServices = data);
    this.service.getTotalRevenue().subscribe(data => this.totalRevenue = data);
    this.service.getAverageServiceCost().subscribe(data => this.averageServiceCost = data);
    this.service.getTopCustomers().subscribe(data => this.topCustomers = data);
    this.service.getMechanicPerformance().subscribe(data => this.mechanicPerformance = data);
    this.service.getLowStockParts().subscribe(data => this.lowStockParts = data);
    this.service.getServiceTypeAnalysis().subscribe(data => this.serviceTypeAnalysis = data);
    this.service.getTopMechanicsByRevenue().subscribe(data => this.topMechanicsByRevenue = data);
    this.service.getMonthlyRevenueAndServiceCount().subscribe(data => this.monthlyRevenueAndServiceCount = data);
  }
}