import { Component } from '@angular/core';
import { CreateOrganization } from '../../components/auth/create-organization/create-organization';

@Component({
  selector: 'app-create-org-page',
  imports: [CreateOrganization],
  templateUrl: './create-org-page.html',
  styleUrl: './create-org-page.css'
})
export class CreateOrgPage {

}
