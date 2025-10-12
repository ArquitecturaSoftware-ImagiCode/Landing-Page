import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CircleStar, LucideAngularModule} from 'lucide-angular';

@Component({
  selector: 'app-header',
  imports: [LucideAngularModule, RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header {

  readonly StarIcon = CircleStar;

}
