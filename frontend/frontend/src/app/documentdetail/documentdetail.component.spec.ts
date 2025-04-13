import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentdetailComponent } from './documentdetail.component';

describe('DocumentdetailComponent', () => {
  let component: DocumentdetailComponent;
  let fixture: ComponentFixture<DocumentdetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentdetailComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DocumentdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
