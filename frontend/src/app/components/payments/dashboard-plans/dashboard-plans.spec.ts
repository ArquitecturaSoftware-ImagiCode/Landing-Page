import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPlans } from './dashboard-plans';

describe('DashboardPlans', () => {
  let component: DashboardPlans;
  let fixture: ComponentFixture<DashboardPlans>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardPlans]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardPlans);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
