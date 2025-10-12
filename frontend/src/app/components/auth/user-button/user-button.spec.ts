import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserButton } from './user-button';

describe('UserButton', () => {
  let component: UserButton;
  let fixture: ComponentFixture<UserButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
