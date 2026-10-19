import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CandidateService } from '../../core/services/candidate.service';

@Component({
  selector: 'app-candidate-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './candidate-layout.html',
  styleUrl: './candidate-layout.css'
})
export class CandidateLayout {
  private auth = inject(AuthService);
  private candidates = inject(CandidateService);

  // Shows the email until the profile name has loaded.
  candidateName = signal(this.auth.user()?.email ?? 'Candidate');

  constructor() {
    this.candidates.getMe().subscribe({
      next: profile => {
        if (profile?.fullName) this.candidateName.set(profile.fullName);
      },
      error: () => { /* keep the email fallback */ },
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
