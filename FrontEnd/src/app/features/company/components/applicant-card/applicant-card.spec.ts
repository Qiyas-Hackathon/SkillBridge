import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ApplicantCard } from './applicant-card';

describe('ApplicantCard', () => {
  let fixture: ComponentFixture<ApplicantCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantCard],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ApplicantCard);
    fixture.componentRef.setInput('applicant', { id: 1, jobId: 1, name: 'A', email: 'a@b.c', skills: [], matchScore: 1, status: 'pending', appliedAt: '2026-10-01', experience: '', bio: '' });
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
