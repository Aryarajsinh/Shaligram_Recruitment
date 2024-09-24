import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsbatchComponent } from './detailsbatch.component';

describe('DetailsbatchComponent', () => {
  let component: DetailsbatchComponent;
  let fixture: ComponentFixture<DetailsbatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailsbatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailsbatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
