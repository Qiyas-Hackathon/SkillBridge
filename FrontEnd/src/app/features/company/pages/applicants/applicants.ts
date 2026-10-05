import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApplicantCard } from '../../components/applicant-card/applicant-card';
import { Applicant, Job } from '../../models/company.model';
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
  private route = inject(ActivatedRoute);

  job?: Job;
  applicants: Applicant[] = [];

  constructor() {
    const jobId = Number(this.route.snapshot.paramMap.get('jobId'));
    this.service.getJob(jobId).subscribe(j => (this.job = j));
    this.service.getApplicants(jobId).subscribe(a => (this.applicants = a));
  }
}