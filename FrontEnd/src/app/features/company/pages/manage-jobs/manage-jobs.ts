import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Job } from '../../../../core/models/api.models';
import { EmployerService } from '../../../../core/services/employer.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  selector: 'app-manage-jobs',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './manage-jobs.html',
  styleUrl: './manage-jobs.css',
})
export class ManageJobs {
  private jobsApi = inject(JobsService);
  private employers = inject(EmployerService);

  jobs = signal<Job[]>([]);
  loading = signal(true);
  error = signal('');

  constructor() {
    this.load();
  }

  load() {
    this.loading.set(true);
    // GET /api/jobs returns every job, so keep only this employer's.
    forkJoin({ profile: this.employers.getMe(), jobs: this.jobsApi.getAll() }).subscribe({
      next: ({ profile, jobs }) => {
        this.jobs.set(profile ? jobs.filter(j => j.employerProfileId === profile.id) : []);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load your jobs.'));
        this.loading.set(false);
      },
    });
  }

  remove(job: Job) {
    if (!confirm(`Delete "${job.title}"?`)) return;
    this.jobsApi.delete(job.id).subscribe({
      next: () => this.jobs.update(list => list.filter(j => j.id !== job.id)),
      error: err => this.error.set(errorMessage(err, 'Could not delete that job.')),
    });
  }
}
