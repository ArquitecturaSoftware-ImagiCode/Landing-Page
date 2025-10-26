import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ClerkService } from 'ngx-clerk';
import { filter, switchMap, finalize } from 'rxjs/operators';
import { Router, RouterModule } from '@angular/router';
import { backendUrl } from '../../../environments/environments';

interface Module {
  id: number;
  name: string;
  description: string;
  isEnabled: boolean;
  activatedAt?: string;
}

interface SubscriptionStatus {
  isActive: boolean;
  planName: string;
  expiresAt?: string;
  status: string;
  isExpiringSoon: boolean;
}

interface Notification {
  id: number;
  title: string;
  message: string;
  isRead: boolean;
  isImportant: boolean;
  createdAt: string;
}

interface OrganizationDashboardData {
  organization: any;
  enabledModules: Module[];
  subscriptionStatus: SubscriptionStatus;
  notifications: Notification[];
  unreadNotificationsCount: number;
}

@Component({
  selector: 'app-organization-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './organization-dashboard.html'
})
export class OrganizationDashboard implements OnInit {
  private http = inject(HttpClient);
  private clerkService = inject(ClerkService);
  private router = inject(Router);

  organization: any = null;
  clerkOrganization: any = null;
  modules: Module[] = [];
  subscriptionStatus: SubscriptionStatus | null = null;
  notifications: Notification[] = [];
  unreadCount = 0;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.clerkService.organization$
      .pipe(
        filter((org) => !!org),
        switchMap((org) => {
          console.log('🔹 Organización actual de Clerk:', org);
          this.clerkOrganization = org; // Guardar la organización de Clerk
          const clerkOrgId = org.id;
          console.log('🆔 Clerk Organization ID:', clerkOrgId);
          console.log('🖼️ Image URL:', org.imageUrl);
          return this.loadDashboardDataFromBackend(clerkOrgId);
        }),
        finalize(() => (this.isLoading = false))
      )
      .subscribe({
        next: () => console.log('✅ Dashboard cargado correctamente'),
        error: (err) => {
          console.error('❌ Error al cargar dashboard:', err);
          this.errorMessage = 'Error al cargar datos del dashboard';
        }
      });
  }

  private loadDashboardDataFromBackend(clerkOrgId: string) {
    this.isLoading = true;
    const url = `${backendUrl}/api/organizationdashboard/dashboard-data?clerkOrgId=${clerkOrgId}`;
    
    console.log('📡 Llamando a:', url);

    return this.http.get<OrganizationDashboardData>(url).pipe(
      switchMap((data) => {
        console.log('✅ Datos recibidos del backend:', data);
        this.organization = data.organization;
        this.modules = data.enabledModules;
        this.subscriptionStatus = data.subscriptionStatus;
        this.notifications = data.notifications;
        this.unreadCount = data.unreadNotificationsCount;
        this.isLoading = false;
        return [];
      })
    );
  }

  markAsRead(notificationId: number): void {
    const notification = this.notifications.find(n => n.id === notificationId);
    if (notification && !notification.isRead) {
      notification.isRead = true;
      this.unreadCount = Math.max(0, this.unreadCount - 1);
    }
  }

  goToPayments(): void {
    this.router.navigate(['/admin/payments']);
  }
}

