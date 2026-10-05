import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApplicantCard } from '../../components/applicant-card/applicant-card';
import { CompanyService } from '../../services/company.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AsyncPipe, RouterLink, ApplicantCard],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  stats$ = inject(CompanyService).getStats();
}