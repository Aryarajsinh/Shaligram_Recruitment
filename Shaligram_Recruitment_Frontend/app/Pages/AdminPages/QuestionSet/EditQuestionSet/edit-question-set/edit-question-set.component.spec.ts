import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditQuestionSetComponent } from './edit-question-set.component';

describe('EditQuestionSetComponent', () => {
  let component: EditQuestionSetComponent;
  let fixture: ComponentFixture<EditQuestionSetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditQuestionSetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditQuestionSetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
