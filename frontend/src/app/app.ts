import { Component, OnInit, signal, OnDestroy } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { ClerkService } from 'ngx-clerk';
import { AuthService, RegisterRequest } from './services/auth.service';
import { Subscription, combineLatest } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit, OnDestroy {
  protected readonly title = signal('frontend');
  private subscriptions: Subscription[] = [];
  private isRegistering = false; // 🔒 Flag para evitar llamadas repetidas

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
    const combinedSub = combineLatest([
      this._clerk.user$,
      this._clerk.organization$,
    ]).subscribe(async ([user, org]) => {
      const currentUrl = this.router.url;

      // Si hay sesión y está en /login o /register → redirigir al dashboard
      if (user && (currentUrl.includes('/login') || currentUrl.includes('/register'))) {
        this.router.navigate(['/admin/dashboard']);
        return;
      }

      // Si el usuario está autenticado → manejamos flujo
      if (user) {
        await this.handleUserFlow(user, org);
      }
    });

    this.subscriptions.push(combinedSub);
  }

  private async handleUserFlow(user: any, org: any) {
    const hasRegistered = localStorage.getItem('hasRegistered');
    const savedClerkId = localStorage.getItem('clerkUserId');

    // ✅ Evita duplicar registro
    if (this.isRegistering) return;

    // ✅ Si ya se registró correctamente
    if (hasRegistered === 'true' && savedClerkId === user.id) return;

    // Marcamos que estamos registrando
    this.isRegistering = true;

    try {
      if (org) {
        await this.registerUserAndOrganization(user, org);
        this.markRegistered(user.id);
        this.router.navigate(['/admin/dashboard']);
      } else {
        const memberships = await user.organizationMemberships;

        if (memberships && memberships.length > 0) {
          const orgMember = memberships[0].organization;
          await this.registerUserAndOrganization(user, orgMember);
          this.markRegistered(user.id);
          this.router.navigate(['/admin/dashboard']);
        } else {
          // 🆕 No tiene ninguna organización
          console.log('🆕 Usuario sin organización, redirigiendo...');
          this.router.navigate(['/create-organization']);
        }
      }
    } finally {
      // 🔓 Liberamos el flag después de terminar
      this.isRegistering = false;
    }
  }

  private async registerUserAndOrganization(user: any, org: any): Promise<void> {
    try {
      const registerRequest: RegisterRequest = {
        clerkUserId: user.id,
        email:
          user.primaryEmailAddress?.emailAddress ||
          user.emailAddresses[0]?.emailAddress,
        organizationName: org.name,
        clerkOrgId: org.id,
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

  private markRegistered(clerkId: string) {
    localStorage.setItem('hasRegistered', 'true');
    localStorage.setItem('clerkUserId', clerkId);
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }
}
