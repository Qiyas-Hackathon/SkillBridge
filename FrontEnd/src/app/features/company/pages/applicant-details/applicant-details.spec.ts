import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ApplicantDetails } from './applicant-details';

describe('ApplicantDetails', () => {
  let fixture: ComponentFixture<ApplicantDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantDetails],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ApplicantDetails);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
