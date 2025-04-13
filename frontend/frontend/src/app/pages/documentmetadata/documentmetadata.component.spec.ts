import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentmetadataComponent } from './documentmetadata.component';

describe('DocumentmetadataComponent', () => {
  let component: DocumentmetadataComponent;
  let fixture: ComponentFixture<DocumentmetadataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocumentmetadataComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DocumentmetadataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
