import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollegebatchComponent } from './collegebatch.component';

describe('CollegebatchComponent', () => {
  let component: CollegebatchComponent;
  let fixture: ComponentFixture<CollegebatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollegebatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollegebatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
