import { Component, OnInit } from '@angular/core';
import { StripeService } from '../../../services/stripe.service';
import { CommonModule } from '@angular/common';
import { ClerkService } from 'ngx-clerk';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard-plans',
  imports: [CommonModule],
  templateUrl: './dashboard-plans.html',
  styleUrl: './dashboard-plans.css',
})
export class DashboardPlans implements OnInit {
  products: any[] = [];
  currentOrganization: any = null;
  currentUser: any = null;

  constructor(private stripeService: StripeService, private clerkService: ClerkService) {}

  ngOnInit() {
    this.loadUserAndOrganization();
    this.loadProducts();
  }

  loadUserAndOrganization() {
    // Obtener el usuario actual
    this.clerkService.user$.pipe(take(1)).subscribe((user) => {
      this.currentUser = user;
    });

    // Obtener la organización actual
    this.clerkService.organization$.pipe(take(1)).subscribe((org) => {
      console.log(org);
      this.currentOrganization = org;
    });
  }

  loadProducts() {
    this.stripeService.getAllProducts().subscribe((res: any) => {
      this.products = res.data;
    });
  }

  subscribe(priceId: string) {
    if (!this.currentOrganization || !this.currentUser) {
      console.error('No se pudo obtener la organización o usuario');
      return;
    }

    const organizationId = this.getOrganizationId();
    const customerEmail = this.getUserEmail();
    const customerName = this.getUserName();

    if (!organizationId) {
      console.error('No se pudo obtener el organizationId');
      return;
    }

    console.log('Datos a enviar:', {
      priceId,
      organizationId,
      customerEmail,
      customerName,
    });

    this.stripeService
      .createSubscription(priceId, organizationId, customerEmail, customerName)
      .subscribe(
        (res) => {
          window.location.href = res.url; // redirige al checkout
        },
        (error) => {
          console.error('Error al crear suscripción:', error);
        }
      );
  }

  private getOrganizationId(): any {
    // Dependiendo de cómo tengas la estructura de la organización en Clerk
    // Puedes obtener el ID de diferentes maneras:
    return this.currentOrganization?.id;
  }

  private getUserEmail(): string {
    return (
      this.currentUser?.primaryEmailAddress?.emailAddress ||
      this.currentUser?.emailAddresses[0]?.emailAddress ||
      ''
    );
  }

  private getUserName(): string {
    return (
      this.currentUser?.fullName ||
      `${this.currentUser?.firstName} ${this.currentUser?.lastName}` ||
      'Cliente'
    );
  }
}
