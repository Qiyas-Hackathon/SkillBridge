import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Job } from '../../../../core/models/api.models';
import { CandidateService } from '../../../../core/services/candidate.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { errorMessage } from '../../../../core/utils/errors';
import { matchScore } from '../../../../core/utils/match';
import { MatchScore } from '../../components/match-score/match-score';
import { SkillList } from '../../components/skill-list/skill-list';

@Component({
  selector: 'app-job-details',
  imports: [SkillList, MatchScore, RouterLink, DatePipe],
  templateUrl: './job-details.html',
  styleUrl: './job-details.css'
})
export class JobDetails {
  private route = inject(ActivatedRoute);
  private jobsApi = inject(JobsService);
  private candidates = inject(CandidateService);

  job = signal<Job | null>(null);
  skillNames = signal<string[]>([]);
  match = signal(0);
  loading = signal(true);
  error = signal('');

  constructor() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.jobsApi.get(id).subscribe({
      next: job => {
        this.job.set(job);
        this.skillNames.set(job.requiredSkills.map(s => s.name));
        this.loading.set(false);

        // Match score needs the candidate's skills; a missing profile just means 0%.
        this.candidates.getMe().subscribe({
          next: profile => this.match.set(matchScore(job, profile?.skills ?? [])),
          error: () => this.match.set(0),
        });
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load this job.'));
        this.loading.set(false);
      },
    });
  }
}
