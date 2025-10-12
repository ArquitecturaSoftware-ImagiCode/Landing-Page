import { Component } from '@angular/core';
import { CircleStar, LucideAngularModule } from 'lucide-angular';

@Component({
  selector: 'app-ceo',
  imports: [LucideAngularModule],
  templateUrl: './ceo.html',
  styleUrl: './ceo.css'
})
export class Ceo {
  readonly StarIcon = CircleStar;

}
