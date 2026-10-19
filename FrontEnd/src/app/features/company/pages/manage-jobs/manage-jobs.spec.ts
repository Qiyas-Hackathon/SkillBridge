import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ManageJobs } from './manage-jobs';

describe('ManageJobs', () => {
  let fixture: ComponentFixture<ManageJobs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageJobs],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ManageJobs);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
