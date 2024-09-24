import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionsetComponent } from './questionset.component';

describe('QuestionsetComponent', () => {
  let component: QuestionsetComponent;
  let fixture: ComponentFixture<QuestionsetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuestionsetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionsetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
