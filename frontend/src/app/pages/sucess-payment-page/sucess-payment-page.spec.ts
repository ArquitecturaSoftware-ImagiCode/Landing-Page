import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SucessPaymentPage } from './sucess-payment-page';

describe('SucessPaymentPage', () => {
  let component: SucessPaymentPage;
  let fixture: ComponentFixture<SucessPaymentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SucessPaymentPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SucessPaymentPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
