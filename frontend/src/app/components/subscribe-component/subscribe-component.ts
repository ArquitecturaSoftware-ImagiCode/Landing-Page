// import { Component, OnInit } from '@angular/core';
// import { StripeService } from '../../services/stripe.service';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-subscribe-component',
//   imports: [CommonModule],
//   templateUrl: './subscribe-component.html',
//   styleUrl: './subscribe-component.css'
// })
// export class SubscribeComponent implements OnInit {
//   products: any[] = [];

//   constructor(private stripeService: StripeService) {}

//   ngOnInit() {
//     this.loadProducts();
//   }

//   loadProducts() {
//     this.stripeService.getAllProducts().subscribe((res: any) => {
//       this.products = res.data;
//     });
//   }

//   subscribe(priceId: string) {
//     this.stripeService.createSubscription(priceId).subscribe((res) => {
//       window.location.href = res.url; // redirige al checkout
//     });
//   }
// }
