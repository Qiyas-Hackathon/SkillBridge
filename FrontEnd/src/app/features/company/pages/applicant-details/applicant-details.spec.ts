import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { ApplicantDetails } from './applicant-details';

function setup(id: string) {
  TestBed.configureTestingModule({
    imports: [ApplicantDetails],
    providers: [
      provideRouter([]),
      { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap({ id }) } } },
    ],
  });
  const fixture: ComponentFixture<ApplicantDetails> = TestBed.createComponent(ApplicantDetails);
  fixture.detectChanges();
  return fixture;
}

describe('ApplicantDetails', () => {
  it('shows the applicant profile', () => {
    const fixture = setup('1');
    expect(fixture.nativeElement.querySelector('h1').textContent).toContain('Abel Tesfaye');
  });

  it('updates the status', () => {
    const fixture = setup('1');
    fixture.componentInstance.updateStatus('hired');
    expect(fixture.componentInstance.applicant?.status).toBe('hired');
  });

  it('shows a message when the applicant does not exist', () => {
    const fixture = setup('999');
    expect(fixture.nativeElement.textContent).toContain('Applicant not found');
  });
});