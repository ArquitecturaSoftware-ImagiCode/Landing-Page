import { Component } from '@angular/core';
import { Header } from '../../components/landing/header/header';
import { Features } from '../../components/landing/features/features';
import { Ceo } from '../../components/landing/ceo/ceo';
import { PaymentsLanding } from '../../components/landing/payments-landing/payments-landing';
import { Integration } from '../../components/landing/integration/integration';
import { Banner } from '../../components/landing/banner/banner';
import { Footer } from '../../components/landing/footer/footer';

@Component({
  selector: 'app-landing-page',
  imports: [Header, Features, Ceo, PaymentsLanding, Integration, Banner, Footer],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css'
})
export class LandingPage {



}
