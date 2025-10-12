import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentsDashboard } from './payments-dashboard';

describe('PaymentsDashboard', () => {
  let component: PaymentsDashboard;
  let fixture: ComponentFixture<PaymentsDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentsDashboard]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentsDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
