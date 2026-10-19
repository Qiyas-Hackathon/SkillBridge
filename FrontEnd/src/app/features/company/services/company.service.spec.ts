import { TestBed } from '@angular/core/testing';
import { CompanyService } from './company.service';

describe('CompanyService', () => {
  let service: CompanyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompanyService);
  });

  it('returns no applicants until the API supports applications', () => {
    service.getApplicants(1).subscribe(list => expect(list).toEqual([]));
    service.getApplicant(1).subscribe(a => expect(a).toBeUndefined());
  });
});
