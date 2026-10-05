import { Component, EventEmitter, Input, Output } from '@angular/core';
import { APPLICATION_STATUSES, ApplicationStatus } from '../../models/company.model';

@Component({
  selector: 'app-applicant-status',
  standalone: true,
  templateUrl: './applicant-status.html',
  styleUrl: './applicant-status.css',
})
export class ApplicantStatus {
  @Input() status: ApplicationStatus = 'pending';
  @Input() editable = false;
  @Output() changed = new EventEmitter<ApplicationStatus>();

  statuses = APPLICATION_STATUSES;

  onChange(event: Event) {
    this.changed.emit((event.target as HTMLSelectElement).value as ApplicationStatus);
  }
}