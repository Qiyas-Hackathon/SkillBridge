import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobForm } from '../../components/job-form/job-form';
import { Job, JobInput } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [JobForm],
  templateUrl: './post-job.html',
  styleUrl: './post-job.css',
})
export class PostJob {
  private service = inject(CompanyService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  jobId = Number(this.route.snapshot.paramMap.get('id')) || null;
  job: Job | null = null;

  constructor() {
    if (this.jobId) {
      this.service.getJob(this.jobId).subscribe(j => (this.job = j ?? null));
    }
  }

  save(input: JobInput) {
    const request = this.jobId
      ? this.service.updateJob(this.jobId, input)
      : this.service.addJob(input);
    request.subscribe(() => this.router.navigate(['/company/jobs']));
  }
}