import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { JobSearch } from './job-search';

describe('JobSearch', () => {
  let fixture: ComponentFixture<JobSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobSearch],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(JobSearch);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
