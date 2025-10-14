import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentsLanding } from './payments-landing';

describe('PaymentsLanding', () => {
  let component: PaymentsLanding;
  let fixture: ComponentFixture<PaymentsLanding>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentsLanding]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentsLanding);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
