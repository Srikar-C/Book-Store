import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookentryComponent } from './bookentry.component';

describe('BookentryComponent', () => {
  let component: BookentryComponent;
  let fixture: ComponentFixture<BookentryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookentryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookentryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
