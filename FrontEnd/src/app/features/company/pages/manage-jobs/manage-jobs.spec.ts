import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { vi } from 'vitest';
import { ManageJobs } from './manage-jobs';

describe('ManageJobs', () => {
  let fixture: ComponentFixture<ManageJobs>;
  let component: ManageJobs;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageJobs],
      providers: [provideRouter([])],
    }).compileComponents();
    fixture = TestBed.createComponent(ManageJobs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it('renders a row for each job', () => {
    expect(fixture.nativeElement.querySelectorAll('tbody tr').length).toBe(3);
  });

  it('toggles a job between open and closed', () => {
    const job = component.jobs[0];
    expect(job.status).toBe('open');
    component.toggle(job);
    expect(component.jobs.find(j => j.id === job.id)?.status).toBe('closed');
  });

  it('deletes a job after confirmation', () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true);
    component.remove(component.jobs[0]);
    expect(component.jobs.length).toBe(2);
  });

  it('keeps the job when deletion is cancelled', () => {
    vi.spyOn(window, 'confirm').mockReturnValue(false);
    component.remove(component.jobs[0]);
    expect(component.jobs.length).toBe(3);
  });
});