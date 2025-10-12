import { Component, OnInit } from '@angular/core';
import { DashboardComponent } from '../../components/dashboard/dashboard-component/dashboard-component';
import { ClerkService } from 'ngx-clerk';
import { HttpClient } from '@angular/common/http';
import { backendUrl } from '../../environments/environments';
import { CommonModule } from '@angular/common';
import { Check, Heart, LucideAngularModule, TriangleAlert } from 'lucide-angular';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, LucideAngularModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit {

   readonly Alert = TriangleAlert;
  readonly HeartIcon = Heart;
  readonly CheckIcon = Check;

  organization: any = null;
  loading = true;

  constructor(
    private clerk: ClerkService,
    private http: HttpClient
  ) {}

  ngOnInit() {
    // 🧠 Suscribirse al observable de Clerk (nunca usar toPromise aquí)
    this.clerk.organization$.subscribe({
      next: (org) => {
        console.log('🔹 Organización actual de Clerk:', org);

        if (org) {
          const clerkOrgId = org.id;
          console.log('🆔 Clerk Organization ID:', clerkOrgId);

          // 🧠 Consultar la organización en tu backend
          this.http.get(`${backendUrl}/organization/${clerkOrgId}`).subscribe({
            next: (res) => {
              console.log('✅ Organización desde backend:', res);
              this.organization = res;
              this.loading = false;
            },
            error: (err) => {
              console.error('❌ Error al obtener organización del backend:', err);
              this.loading = false;
            }
          });
        } else {
          console.warn('⚠️ No se encontró ninguna organización activa en Clerk.');
          this.loading = false;
        }
      },
      error: (err) => {
        console.error('❌ Error al escuchar organización de Clerk:', err);
        this.loading = false;
      }
    });
  }
}
