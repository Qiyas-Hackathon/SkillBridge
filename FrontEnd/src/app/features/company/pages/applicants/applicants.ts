import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Job } from '../../../../core/models/api.models';
import { JobsService } from '../../../../core/services/jobs.service';
import { ApplicantCard } from '../../components/applicant-card/applicant-card';
import { Applicant } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-applicants',
  standalone: true,
  imports: [ApplicantCard, RouterLink],
  templateUrl: './applicants.html',
  styleUrl: './applicants.css',
})
export class Applicants {
  private service = inject(CompanyService);
  private jobsApi = inject(JobsService);
  private route = inject(ActivatedRoute);

  job = signal<Job | null>(null);
  applicants = signal<Applicant[]>([]);

  constructor() {
    const jobId = Number(this.route.snapshot.paramMap.get('jobId')) || undefined;
    if (jobId) this.jobsApi.get(jobId).subscribe({ next: j => this.job.set(j), error: () => {} });
    this.service.getApplicants(jobId).subscribe(a => this.applicants.set(a));
  }
}
