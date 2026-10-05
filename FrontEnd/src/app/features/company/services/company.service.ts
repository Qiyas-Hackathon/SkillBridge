import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import {
  Applicant, ApplicationStatus, CompanyStats, Job, JobInput,
} from '../models/company.model';

@Injectable({ providedIn: 'root' })
export class CompanyService {
  private jobs: Job[] = [
    {
      id: 1, title: 'Frontend Developer', location: 'Addis Ababa', type: 'Full-time',
      description: 'Build modern Angular interfaces for our platform.',
      skills: ['Angular', 'TypeScript', 'CSS'], salary: '30,000 ETB',
      deadline: '2026-11-15', status: 'open', postedAt: '2026-09-28',
    },
    {
      id: 2, title: 'Backend Intern', location: 'Remote', type: 'Internship',
      description: 'Help build REST APIs and learn from senior engineers.',
      skills: ['Node.js', 'SQL'], deadline: '2026-10-30', status: 'open', postedAt: '2026-10-01',
    },
    {
      id: 3, title: 'UI/UX Designer', location: 'Addis Ababa', type: 'Contract',
      description: 'Design user flows and prototypes.',
      skills: ['Figma', 'Research'], salary: '20,000 ETB',
      deadline: '2026-09-20', status: 'closed', postedAt: '2026-08-25',
    },
  ];

  private applicants: Applicant[] = [
    { id: 1, jobId: 1, name: 'Abel Tesfaye', email: 'abel@mail.com', skills: ['Angular', 'TypeScript'],
      matchScore: 88, status: 'pending', appliedAt: '2026-10-02', experience: '2 years',
      bio: 'Frontend developer who loves clean UI and component design.' },
    { id: 2, jobId: 1, name: 'Selam Bekele', email: 'selam@mail.com', skills: ['Angular', 'CSS', 'RxJS'],
      matchScore: 94, status: 'shortlisted', appliedAt: '2026-10-03', experience: '3 years',
      bio: 'Built several production Angular apps for fintech teams.' },
    { id: 3, jobId: 2, name: 'Dawit Alemu', email: 'dawit@mail.com', skills: ['Node.js', 'SQL'],
      matchScore: 72, status: 'reviewed', appliedAt: '2026-10-04', experience: 'Student',
      bio: 'Final-year CS student interested in backend systems.' },
    { id: 4, jobId: 3, name: 'Hana Girma', email: 'hana@mail.com', skills: ['Figma'],
      matchScore: 81, status: 'hired', appliedAt: '2026-09-10', experience: '4 years',
      bio: 'Product designer with a strong portfolio.' },
  ];

  private nextJobId = 4;

  getJobs(): Observable<Job[]> {
    return of([...this.jobs]);
  }

  getJob(id: number): Observable<Job | undefined> {
    return of(this.jobs.find(j => j.id === id));
  }

  addJob(input: JobInput): Observable<Job> {
    const job: Job = {
      ...input,
      id: this.nextJobId++,
      status: 'open',
      postedAt: new Date().toISOString().slice(0, 10),
    };
    this.jobs.unshift(job);
    return of(job);
  }

  updateJob(id: number, changes: Partial<Job>): Observable<Job | undefined> {
    const job = this.jobs.find(j => j.id === id);
    if (job) Object.assign(job, changes);
    return of(job);
  }

  deleteJob(id: number): Observable<void> {
    this.jobs = this.jobs.filter(j => j.id !== id);
    this.applicants = this.applicants.filter(a => a.jobId !== id);
    return of(undefined);
  }

  getApplicants(jobId?: number): Observable<Applicant[]> {
    const list = jobId ? this.applicants.filter(a => a.jobId === jobId) : this.applicants;
    return of([...list]);
  }

  getApplicant(id: number): Observable<Applicant | undefined> {
    return of(this.applicants.find(a => a.id === id));
  }

  updateApplicantStatus(id: number, status: ApplicationStatus): Observable<Applicant | undefined> {
    const applicant = this.applicants.find(a => a.id === id);
    if (applicant) applicant.status = status;
    return of(applicant);
  }

  getStats(): Observable<CompanyStats> {
    return of({
      activeJobs: this.jobs.filter(j => j.status === 'open').length,
      totalApplicants: this.applicants.length,
      shortlisted: this.applicants.filter(a => a.status === 'shortlisted').length,
      recent: [...this.applicants]
        .sort((a, b) => b.appliedAt.localeCompare(a.appliedAt))
        .slice(0, 3),
    });
  }
}