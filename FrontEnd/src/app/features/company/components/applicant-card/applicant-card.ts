import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Applicant } from '../../models/company.model';
import { ApplicantStatus } from '../applicant-status/applicant-status';

@Component({
  selector: 'app-applicant-card',
  standalone: true,
  imports: [RouterLink, ApplicantStatus],
  templateUrl: './applicant-card.html',
  styleUrl: './applicant-card.css',
})
export class ApplicantCard {
  @Input({ required: true }) applicant!: Applicant;
}