import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { ApplicantsComponent } from './applicants';

describe('ApplicantsComponent', () => {
  let fixture: ComponentFixture<ApplicantsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantsComponent],
      providers: [
        provideRouter([]),
        { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap({ jobId: '1' }) } } },
      ],
    }).compileComponents();
    fixture = TestBed.createComponent(ApplicantsComponent);
    fixture.detectChanges();
  });

  it('shows the job title and its applicants only', () => {
    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('h1')?.textContent).toContain('Frontend Developer');
    expect(el.querySelectorAll('app-applicant-card').length).toBe(2);
  });
});