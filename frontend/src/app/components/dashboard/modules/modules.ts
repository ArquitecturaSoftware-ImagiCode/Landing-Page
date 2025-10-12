import { Component } from '@angular/core';
import { LucideAngularModule, Plus } from 'lucide-angular';

@Component({
  selector: 'app-modules',
  imports: [LucideAngularModule],
  templateUrl: './modules.html',
  styleUrl: './modules.css'
})
export class Modules {
  readonly PlusIcon = Plus;
}
