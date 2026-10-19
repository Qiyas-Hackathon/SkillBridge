import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Job, JobRequest } from '../../../../core/models/api.models';
import { JobsService } from '../../../../core/services/jobs.service';
import { errorMessage } from '../../../../core/utils/errors';
import { JobForm } from '../../components/job-form/job-form';

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [JobForm],
  templateUrl: './post-job.html',
  styleUrl: './post-job.css',
})
export class PostJob {
  private jobsApi = inject(JobsService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  jobId = Number(this.route.snapshot.paramMap.get('id')) || null;
  job: Job | null = null;
  saving = signal(false);
  error = signal('');

  constructor() {
    if (this.jobId) {
      this.jobsApi.get(this.jobId).subscribe({
        next: j => (this.job = j),
        error: err => this.error.set(errorMessage(err, 'Could not load this job.')),
      });
    }
  }

  save(input: JobRequest) {
    this.saving.set(true);
    this.error.set('');

    const request = this.jobId
      ? this.jobsApi.update(this.jobId, input)
      : this.jobsApi.create(input);

    request.subscribe({
      next: () => this.router.navigate(['/company/jobs']),
      error: err => {
        this.saving.set(false);
        this.error.set(errorMessage(err, 'Could not save the job.'));
      },
    });
  }
}
