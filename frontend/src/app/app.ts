import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { ClerkService } from 'ngx-clerk';
import { AuthService, RegisterRequest } from './services/auth.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  protected readonly title = signal('frontend');

  constructor(
    private _clerk: ClerkService,
    private router: Router,
    private authService: AuthService
  ) {
    // Inicializa Clerk
    this._clerk.__init({
      publishableKey:
        'pk_test_Z3JhdGVmdWwtbW9uYXJjaC0xMC5jbGVyay5hY2NvdW50cy5kZXYk',
    });
  }

  ngOnInit() {
    // Espera solo una vez al usuario autenticado
    this._clerk.user$.pipe(take(1)).subscribe(async (user) => {
      if (user) {
        await this.handleUserFlow(user);
      }
    });
  }

  private async handleUserFlow(user: any) {
    const hasRegistered = localStorage.getItem('hasRegistered');
    const savedClerkId = localStorage.getItem('clerkUserId');

    // ✅ Si ya se registró, no hacemos nada
    if (hasRegistered === 'true' && savedClerkId === user.id) {
      return;
    }

    // 🔹 Esperamos a ver si hay una organización activa en Clerk
    this._clerk.organization$.pipe(take(1)).subscribe(async (org) => {
      if (org) {
        // ✅ Ya hay organización activa (posiblemente recién creada)
        await this.registerUserAndOrganization(user, org);
        localStorage.setItem('hasRegistered', 'true');
        localStorage.setItem('clerkUserId', user.id);
        this.router.navigate(['/admin/dashboard']);
      } else {
        // 🔹 Si no hay organización activa, buscamos las que el usuario ya tiene
        const memberships = await user.organizationMemberships;

        if (memberships && memberships.length > 0) {
          // ✅ Usa la primera organización donde el usuario sea miembro/admin
          const orgMember = memberships[0].organization;
          console.log('➡️ Usuario pertenece a organización existente:', orgMember);

          await this.registerUserAndOrganization(user, orgMember);
          localStorage.setItem('hasRegistered', 'true');
          localStorage.setItem('clerkUserId', user.id);
          this.router.navigate(['/admin/dashboard']);
        } else {
          // 🆕 No tiene ninguna → redirigir a crear
          console.log('🆕 Usuario sin organización, redirigiendo...');
          this.router.navigate(['/create-organization']);
        }
      }
    });
  }

  private async registerUserAndOrganization(user: any, org: any): Promise<void> {
    try {
      const registerRequest: RegisterRequest = {
        clerkUserId: user.id,
        email:
          user.primaryEmailAddress?.emailAddress ||
          user.emailAddresses[0]?.emailAddress,
        organizationName: org.name,
        clerkOrgId: org.id, // 🔹 Agregamos el ID de la organización
      };

      const response = await this.authService
        .registerUser(registerRequest)
        .toPromise();

      console.log('✅ Usuario y organización registrados en backend:', response);
      localStorage.setItem('userId', response?.userId?.toString() || '');
    } catch (error) {
      console.error('❌ Error registrando en backend:', error);
    }
  }
}
