import { Component } from '@angular/core';
import { Header } from '../../components/landing/header/header';
import { Features } from '../../components/landing/features/features';
import { Ceo } from '../../components/landing/ceo/ceo';

@Component({
  selector: 'app-landing-page',
  imports: [Header, Features, Ceo],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css'
})
export class LandingPage {



}
