import { Component } from '@angular/core';
import { CircleCheck, CircleStar, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-features',
  imports: [LucideAngularModule],
  templateUrl: './features.html',
  styleUrl: './features.css'
})
export class Features {
    readonly StarIcon = CircleStar;
    readonly CircleCheckIcon = CircleCheck;

}
