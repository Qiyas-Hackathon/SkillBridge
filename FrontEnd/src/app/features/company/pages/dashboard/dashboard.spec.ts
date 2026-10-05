import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Dashboard } from './dashboard';

describe('Dashboard', () => {
  let fixture: ComponentFixture<Dashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dashboard],
      providers: [provideRouter([])],
    }).compileComponents();
    fixture = TestBed.createComponent(Dashboard);
    fixture.detectChanges();
  });

  it('shows the three stat cards', () => {
    expect(fixture.nativeElement.querySelectorAll('.stat').length).toBe(3);
  });

  it('shows recent applicants (max 3)', () => {
    expect(fixture.nativeElement.querySelectorAll('app-applicant-card').length).toBe(3);
  });
});