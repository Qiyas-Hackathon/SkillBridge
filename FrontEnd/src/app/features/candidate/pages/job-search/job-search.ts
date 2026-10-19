import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Subject, catchError, debounceTime, forkJoin, of, switchMap, tap } from 'rxjs';
import { Skill } from '../../../../core/models/api.models';
import { CandidateService } from '../../../../core/services/candidate.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { SkillsService } from '../../../../core/services/skills.service';
import { errorMessage } from '../../../../core/utils/errors';
import { JobCardData, toJobCard } from '../../../../core/utils/match';
import { JobCard } from '../../components/job-card/job-card';

@Component({
  selector: 'app-job-search',
  imports: [FormsModule, JobCard],
  templateUrl: './job-search.html',
  styleUrl: './job-search.css'
})
export class JobSearch {
  private jobsApi = inject(JobsService);
  private skillsApi = inject(SkillsService);
  private candidates = inject(CandidateService);
  private destroyRef = inject(DestroyRef);

  searchTerm = '';
  selectedSkillId: number | null = null;

  skills = signal<Skill[]>([]);
  jobs = signal<JobCardData[]>([]);
  loading = signal(true);
  error = signal('');

  private filters$ = new Subject<void>();
  private candidateSkills: Skill[] = [];

  constructor() {
    // Catalogue for the skill filter + the candidate's own skills for match scores.
    forkJoin({
      skills: this.skillsApi.getAll(),
      profile: this.candidates.getMe().pipe(catchError(() => of(null))),
    }).subscribe({
      next: ({ skills, profile }) => {
        this.skills.set(skills);
        this.candidateSkills = profile?.skills ?? [];
        this.filters$.next();
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load jobs.'));
        this.loading.set(false);
      },
    });

    // Re-query the API whenever the search text or skill filter changes.
    this.filters$
      .pipe(
        debounceTime(300),
        tap(() => { this.loading.set(true); this.error.set(''); }),
        switchMap(() =>
          this.jobsApi
            .getAll({ search: this.searchTerm, skillId: this.selectedSkillId })
            .pipe(
              catchError(err => {
                this.error.set(errorMessage(err, 'Could not load jobs.'));
                return of(null);
              }),
            ),
        ),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(jobs => {
        if (jobs) this.jobs.set(jobs.map(j => toJobCard(j, this.candidateSkills)));
        this.loading.set(false);
      });
  }

  onFilterChange(): void {
    this.filters$.next();
  }
}
