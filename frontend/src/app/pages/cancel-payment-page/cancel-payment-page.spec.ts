import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CancelPaymentPage } from './cancel-payment-page';

describe('CancelPaymentPage', () => {
  let component: CancelPaymentPage;
  let fixture: ComponentFixture<CancelPaymentPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CancelPaymentPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CancelPaymentPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
