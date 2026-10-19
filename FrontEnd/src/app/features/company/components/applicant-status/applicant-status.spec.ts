import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ApplicantStatus } from './applicant-status';

describe('ApplicantStatus', () => {
  let fixture: ComponentFixture<ApplicantStatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantStatus],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ApplicantStatus);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
