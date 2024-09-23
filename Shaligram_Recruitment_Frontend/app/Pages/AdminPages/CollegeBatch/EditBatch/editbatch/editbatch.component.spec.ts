import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditbatchComponent } from './editbatch.component';

describe('EditbatchComponent', () => {
  let component: EditbatchComponent;
  let fixture: ComponentFixture<EditbatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditbatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditbatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
