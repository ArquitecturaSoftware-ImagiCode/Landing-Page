import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Ceo } from './ceo';

describe('Ceo', () => {
  let component: Ceo;
  let fixture: ComponentFixture<Ceo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Ceo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Ceo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
