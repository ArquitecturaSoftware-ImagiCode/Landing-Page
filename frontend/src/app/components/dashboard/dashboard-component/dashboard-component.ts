import { Component } from '@angular/core';
import { Heart, LucideAngularModule, TriangleAlert } from 'lucide-angular';

@Component({
  selector: 'app-dashboard-component',
  imports: [LucideAngularModule],
  templateUrl: './dashboard-component.html',
  styleUrl: './dashboard-component.css'
})
export class DashboardComponent {

  readonly Alert = TriangleAlert;
  readonly HeartIcon = Heart;

}
