import { TestBed } from '@angular/core/testing';
import { CompanyService } from './company.service';

describe('CompanyService', () => {
  let service: CompanyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompanyService);
  });

  it('returns the mock jobs', () => {
    service.getJobs().subscribe(jobs => expect(jobs.length).toBe(3));
  });

  it('adds a new open job', () => {
    service.addJob({
      title: 'QA Engineer', description: 'Test our platform thoroughly.',
      location: 'Remote', type: 'Contract', skills: ['Testing'], deadline: '2026-12-01',
    }).subscribe(job => {
      expect(job.status).toBe('open');
      service.getJobs().subscribe(jobs => expect(jobs.length).toBe(4));
    });
  });

  it('deletes a job together with its applicants', () => {
    service.deleteJob(1).subscribe(() => {
      service.getApplicants(1).subscribe(a => expect(a.length).toBe(0));
    });
  });

  it('filters applicants by job', () => {
    service.getApplicants(1).subscribe(a => expect(a.length).toBe(2));
  });

  it('updates an applicant status', () => {
    service.updateApplicantStatus(1, 'rejected').subscribe(a => expect(a?.status).toBe('rejected'));
  });

  it('computes the dashboard stats', () => {
    service.getStats().subscribe(s => {
      expect(s.activeJobs).toBe(2);
      expect(s.totalApplicants).toBe(4);
      expect(s.shortlisted).toBe(1);
      expect(s.recent.length).toBe(3);
    });
  });
});