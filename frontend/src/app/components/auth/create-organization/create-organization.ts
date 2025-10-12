import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ClerkCreateOrganizationComponent, ClerkService } from 'ngx-clerk';

@Component({
  selector: 'app-create-organization',
  imports: [ClerkCreateOrganizationComponent],
  templateUrl: './create-organization.html',
  styleUrl: './create-organization.css',
})
export class CreateOrganization {
  constructor(private _clerk: ClerkService, private router: Router) {
    this._clerk.__init({
      publishableKey: 'pk_test_Z3JhdGVmdWwtbW9uYXJjaC0xMC5jbGVyay5hY2NvdW50cy5kZXYk',
    });
  }
}
