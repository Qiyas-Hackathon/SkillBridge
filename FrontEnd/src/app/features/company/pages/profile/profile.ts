import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployerProfile } from '../../../../core/models/api.models';
import { EmployerService } from '../../../../core/services/employer.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  selector: 'app-company-profile',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class CompanyProfile {
  private fb = inject(FormBuilder);
  private employers = inject(EmployerService);

  loading = signal(true);
  saving = signal(false);
  saved = signal(false);
  error = signal('');

  /** null means no profile yet; saving will create it. */
  profile = signal<EmployerProfile | null>(null);

  form = this.fb.nonNullable.group({
    companyName: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', Validators.maxLength(1000)],
  });

  constructor() {
    this.employers.getMe().subscribe({
      next: p => {
        this.profile.set(p);
        if (p) this.form.patchValue({ companyName: p.companyName, description: p.description ?? '' });
        this.loading.set(false);
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load the company profile.'));
        this.loading.set(false);
      },
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    const body = { companyName: v.companyName.trim(), description: v.description.trim() || null };

    this.saving.set(true);
    this.saved.set(false);
    this.error.set('');

    const request = this.profile() ? this.employers.update(body) : this.employers.create(body);
    request.subscribe({
      next: p => {
        this.profile.set(p);
        this.saving.set(false);
        this.saved.set(true);
      },
      error: err => {
        this.saving.set(false);
        this.error.set(errorMessage(err, 'Could not save the company profile.'));
      },
    });
  }
}
