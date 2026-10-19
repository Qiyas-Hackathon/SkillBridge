import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CandidateProfile, Job } from '../../../../core/models/api.models';
import { CandidateService } from '../../../../core/services/candidate.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { errorMessage } from '../../../../core/utils/errors';
import { JobCardData, toJobCard } from '../../../../core/utils/match';
import { JobCard } from '../../components/job-card/job-card';

@Component({
  selector: 'app-dashboard',
  imports: [JobCard, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {
  private candidates = inject(CandidateService);
  private jobsApi = inject(JobsService);

  loading = signal(true);
  error = signal('');

  candidateName = signal('');
  stats = signal({ openJobs: 0, skills: 0, goodMatches: 0 });
  recommendedJobs = signal<JobCardData[]>([]);

  constructor() {
    forkJoin({
      profile: this.candidates.getMe(),
      jobs: this.jobsApi.getAll(),
    }).subscribe({
      next: ({ profile, jobs }) => this.show(profile, jobs),
      error: err => {
        this.error.set(errorMessage(err, 'Could not load your dashboard.'));
        this.loading.set(false);
      },
    });
  }

  private show(profile: CandidateProfile | null, jobs: Job[]): void {
    const skills = profile?.skills ?? [];
    const cards = jobs
      .map(job => toJobCard(job, skills))
      .sort((a, b) => b.match - a.match);

    this.candidateName.set(profile?.fullName ?? 'there');
    this.stats.set({
      openJobs: jobs.length,
      skills: skills.length,
      goodMatches: cards.filter(c => c.match >= 50).length,
    });
    this.recommendedJobs.set(cards.slice(0, 3));
    this.loading.set(false);
  }
}
