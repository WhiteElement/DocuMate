import { TestBed } from '@angular/core/testing';

import { UploadstateService } from './uploadstate.service';

describe('UploadstateService', () => {
  let service: UploadstateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadstateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
