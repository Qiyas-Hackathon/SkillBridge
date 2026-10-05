import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { ApplicantCard } from './applicant-card';
import { Applicant } from '../../models/company.model';

const applicant: Applicant = {
  id: 1, jobId: 1, name: 'Abel Tesfaye', email: 'abel@mail.com',
  skills: ['Angular', 'TypeScript'], matchScore: 88, status: 'pending',
  appliedAt: '2026-10-02', experience: '2 years', bio: 'Bio',
};

describe('ApplicantCard', () => {
  let fixture: ComponentFixture<ApplicantCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantCard],
      providers: [provideRouter([])],
    }).compileComponents();
    fixture = TestBed.createComponent(ApplicantCard);
    fixture.componentRef.setInput('applicant', applicant);
    fixture.detectChanges();
  });

  it('renders name, score and skills', () => {
    const text = (fixture.nativeElement as HTMLElement).textContent;
    expect(text).toContain('Abel Tesfaye');
    expect(text).toContain('88% match');
    expect(fixture.nativeElement.querySelectorAll('.chip').length).toBe(2);
  });

  it('links to the applicant details page', () => {
    const link = fixture.nativeElement.querySelector('a') as HTMLAnchorElement;
    expect(link.getAttribute('href')).toBe('/company/applicants/1');
  });
});