import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddbatchComponent } from './addbatch.component';

describe('AddbatchComponent', () => {
  let component: AddbatchComponent;
  let fixture: ComponentFixture<AddbatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddbatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddbatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
