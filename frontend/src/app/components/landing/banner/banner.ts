import { Component } from '@angular/core';
import { CircleCheck, CircleStar, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-banner',
  imports: [LucideAngularModule],
  templateUrl: './banner.html',
  styleUrl: './banner.css'
})
export class Banner {
    readonly StarIcon = CircleStar;
    readonly CircleCheckIcon = CircleCheck;
}
