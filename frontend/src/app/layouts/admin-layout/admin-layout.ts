import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { UserButton } from '../../components/auth/user-button/user-button';
import { CircleDollarSign, LayoutDashboard, LucideAngularModule, UserIcon, UsersIcon } from 'lucide-angular';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet, UserButton, LucideAngularModule, RouterModule],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {
  readonly DashboardIcon = LayoutDashboard;
  readonly MoneyIcon = CircleDollarSign;
  readonly UserIcon = UserIcon;
  readonly UsersIcon = UsersIcon;

}
