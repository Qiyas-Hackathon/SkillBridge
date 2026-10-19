import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { JobCard } from './job-card';

describe('JobCard', () => {
  let fixture: ComponentFixture<JobCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobCard],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(JobCard);
    fixture.componentRef.setInput('job', { id: 1, title: 'T', company: 'Acme', postedAt: '2026-10-01', skills: ['C#'], match: 50 });
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
