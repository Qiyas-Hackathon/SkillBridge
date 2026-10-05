import { Component, inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApplicantStatus } from '../../components/applicant-status/applicant-status';
import { Applicant, ApplicationStatus } from '../../models/company.model';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-applicant-details',
  standalone: true,
  imports: [ApplicantStatus, RouterLink, DatePipe],
  templateUrl: './applicant-details.html',
  styleUrl: './applicant-details.css',
})
export class ApplicantDetails {
  private service = inject(CompanyService);
  private route = inject(ActivatedRoute);

  applicant?: Applicant;

  constructor() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.service.getApplicant(id).subscribe(a => (this.applicant = a));
  }

  updateStatus(status: ApplicationStatus) {
    if (!this.applicant) return;
    this.service
      .updateApplicantStatus(this.applicant.id, status)
      .subscribe(a => (this.applicant = a ? { ...a } : this.applicant));
  }
}