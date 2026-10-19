import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { EmployerService } from '../../core/services/employer.service';

@Component({
  selector: 'app-company-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './company-layout.html',
  styleUrl: './company-layout.css',
})
export class CompanyLayout {
  private auth = inject(AuthService);
  private employers = inject(EmployerService);

  companyName = signal(this.auth.user()?.email ?? 'Company');

  constructor() {
    this.employers.getMe().subscribe({
      next: profile => {
        if (profile?.companyName) this.companyName.set(profile.companyName);
      },
      error: () => { /* keep the email fallback */ },
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
