import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  imports: [FormsModule, RouterLink],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  email = '';
  password = '';
  loading = signal(false);
  error = signal('');

  onSubmit(): void {
    if (!this.email.trim() || !this.password) {
      this.error.set('Enter your email and password.');
      return;
    }

    this.loading.set(true);
    this.error.set('');

    this.auth.login({ email: this.email.trim(), password: this.password }).subscribe({
      next: res => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        const home = this.auth.homeFor(res.user.role);
        // Only follow a returnUrl that belongs to this user's area.
        const target = returnUrl && returnUrl.startsWith(home.split('/').slice(0, 2).join('/'))
          ? returnUrl
          : home;
        this.router.navigateByUrl(target);
      },
      error: err => {
        this.loading.set(false);
        this.error.set(errorMessage(err, 'Login failed.'));
      },
    });
  }
}
