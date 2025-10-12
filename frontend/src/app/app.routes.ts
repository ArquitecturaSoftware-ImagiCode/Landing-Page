import { Routes } from '@angular/router';
import { LandingPage } from './pages/landing-page/landing-page';
import { LoginPage } from './pages/login-page/login-page';
import { RegisterPage } from './pages/register-page/register-page';
import { CreateOrganization } from './components/auth/create-organization/create-organization';
import { ClerkAuthGuardService } from 'ngx-clerk';
import { AdminLayout } from './layouts/admin-layout/admin-layout';
import { Dashboard } from './pages/dashboard/dashboard';
import { DashboardPlans } from './components/payments/dashboard-plans/dashboard-plans';
import { Modules } from './components/dashboard/modules/modules';
import { PaymentsDashboard } from './pages/payments-dashboard/payments-dashboard';
import { CreateOrgPage } from './pages/create-org-page/create-org-page';
import { SuccessPayment } from './components/success-payment/success-payment';
import { SucessPaymentPage } from './pages/sucess-payment-page/sucess-payment-page';
import { CancelPaymentPage } from './pages/cancel-payment-page/cancel-payment-page';
import { Users } from './pages/users/users';

export const routes: Routes = [
  { path: '', component: LandingPage },
  { path: 'login', component: LoginPage },
  { path: 'register', component: RegisterPage },
  { path: 'create-organization', component: CreateOrgPage, canActivate: [ClerkAuthGuardService] },
  {
    path: 'admin',
    component: AdminLayout,
    canActivate: [ClerkAuthGuardService],
    children: [
      {
        path: 'dashboard',
        component: Dashboard,
      },
      {
        path: 'modules',
        component: Modules,
      },
      {
        path: "users",
        component: Users

      },
      {
        path: 'payments',
        component: PaymentsDashboard,
      },
      {
        path: 'success',
        component: SucessPaymentPage
      },
      {
        path: 'error',
        component: CancelPaymentPage
      },
    ],
  },
];
