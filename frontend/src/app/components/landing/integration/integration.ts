import { Component } from '@angular/core';
import { CircleCheck, CircleStar, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-integration',
  imports: [LucideAngularModule],
  templateUrl: './integration.html',
  styleUrl: './integration.css'
})
export class Integration {
readonly StarIcon = CircleStar;
    readonly CircleCheckIcon = CircleCheck;
}
