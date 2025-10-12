import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { backendUrl } from '../environments/environments';

@Injectable({ providedIn: 'root' })
export class StripeService {
  private apiUrl = `${backendUrl}/stripe`;

  constructor(private http: HttpClient) {}

  createSubscription(priceId: string, organizationId: number, customerEmail: string, customerName: string) {
    return this.http.post<{ url: string }>(
      `${this.apiUrl}/subscribe`,
      { 
        priceId,
        organizationId,
        customerEmail,
        customerName
      },
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  getAllProducts() {
    return this.http.get(`${this.apiUrl}/GetAllProducts`);
  }
}