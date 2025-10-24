import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CircleStar, LucideAngularModule } from 'lucide-angular';
import { ClerkService } from 'ngx-clerk';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  imports: [CommonModule, LucideAngularModule, RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header implements OnInit, OnDestroy {
  readonly StarIcon = CircleStar;
  isLoggedIn = false;
  private subs?: Subscription;

  constructor(private clerk: ClerkService, private router: Router) {}

  ngOnInit() {
    this.subs = this.clerk.user$.subscribe((user) => {
      this.isLoggedIn = !!user;
    });
  }

  ngOnDestroy() {
    this.subs?.unsubscribe();
  }

  goToDashboard() {
    this.router.navigate(['/admin/dashboard']);
  }
}
