import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { CandidateLayout } from './candidate-layout';

describe('CandidateLayout', () => {
  let fixture: ComponentFixture<CandidateLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidateLayout],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(CandidateLayout);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
