import { Component } from '@angular/core';
import { DashboardPlans } from '../../components/payments/dashboard-plans/dashboard-plans';

@Component({
  selector: 'app-payments-dashboard',
  imports: [DashboardPlans],
  templateUrl: './payments-dashboard.html',
  styleUrl: './payments-dashboard.css'
})
export class PaymentsDashboard {

}
