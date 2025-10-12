import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessPayment } from './success-payment';

describe('SuccessPayment', () => {
  let component: SuccessPayment;
  let fixture: ComponentFixture<SuccessPayment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuccessPayment]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuccessPayment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
