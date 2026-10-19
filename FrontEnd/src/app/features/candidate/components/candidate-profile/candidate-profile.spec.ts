import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { CandidateProfile } from './candidate-profile';

describe('CandidateProfile', () => {
  let fixture: ComponentFixture<CandidateProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidateProfile],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(CandidateProfile);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
