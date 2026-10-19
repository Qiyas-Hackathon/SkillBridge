import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Job } from '../../../../core/models/api.models';
import { EmployerService } from '../../../../core/services/employer.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  private employers = inject(EmployerService);
  private jobsApi = inject(JobsService);

  loading = signal(true);
  error = signal('');
  companyName = signal('');
  myJobs = signal<Job[]>([]);
  skillCount = signal(0);

  constructor() {
    forkJoin({ profile: this.employers.getMe(), jobs: this.jobsApi.getAll() }).subscribe({
      next: ({ profile, jobs }) => {
        const mine = profile ? jobs.filter(j => j.employerProfileId === profile.id) : [];
        this.companyName.set(profile?.companyName ?? '');
        this.myJobs.set(mine);
        this.skillCount.set(new Set(mine.flatMap(j => j.requiredSkills.map(s => s.id))).size);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load the dashboard.'));
        this.loading.set(false);
      },
    });
  }
}
