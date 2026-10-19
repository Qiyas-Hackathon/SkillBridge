import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Job } from '../../../../core/models/api.models';
import { CandidateService } from '../../../../core/services/candidate.service';
import { JobsService } from '../../../../core/services/jobs.service';
import { AuthService } from '../../../../core/services/auth.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  selector: 'app-apply',
  imports: [FormsModule, RouterLink],
  templateUrl: './apply.html',
  styleUrl: './apply.css'
})
export class Apply {
  private route = inject(ActivatedRoute);
  private jobsApi = inject(JobsService);
  private candidates = inject(CandidateService);
  private auth = inject(AuthService);

  job = signal<Job | null>(null);
  error = signal('');
  notice = signal('');

  application = {
    fullName: '',
    email: this.auth.user()?.email ?? '',
    phone: '',
    coverLetter: '',
    portfolioUrl: '',
    resume: ''
  };

  constructor() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.jobsApi.get(id).subscribe({
      next: job => this.job.set(job),
      error: err => this.error.set(errorMessage(err, 'Could not load this job.')),
    });

    // Pre-fill the form from the candidate's profile.
    this.candidates.getMe().subscribe({
      next: profile => {
        if (!profile) return;
        this.application.fullName = profile.fullName;
        this.application.portfolioUrl = profile.portfolioUrl ?? '';
      },
      error: () => { /* the form is still usable */ },
    });
  }

  submitApplication() {
    // The API has no applications endpoint yet, so nothing is sent.
    // Replace this with a POST once the backend supports it.
    this.notice.set(
      'Applications are not available on the server yet, so this application was not submitted.'
    );
  }

}
