import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from '../../../../core/models/api.models';
import { AuthService } from '../../../../core/services/auth.service';
import { errorMessage } from '../../../../core/utils/errors';

type AccountType = 'candidate' | 'company';

// Matches the backend rule: 8+ characters, one digit, one lowercase, one uppercase.
const PASSWORD_RULE = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,128}$/;
const URL_RULE = /^https?:\/\/\S+$/i;

function passwordsMatch(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value;
  const confirm = group.get('confirmPassword')?.value;
  return password === confirm ? null : { mismatch: true };
}

@Component({
  imports: [RouterLink, ReactiveFormsModule],
  standalone: true,
  selector: 'app-register',
  styleUrl: './register.css',
  templateUrl: './register.html',
})
export class Register {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  accountType: AccountType = 'candidate';
  loading = signal(false);
  error = signal('');

  degreeLevels = ['Diploma', "Bachelor's", "Master's", 'PhD'];

  candidateForm = this.fb.nonNullable.group(
    {
      fullName: ['', [Validators.required, Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern(PASSWORD_RULE)]],
      confirmPassword: ['', Validators.required],
      headline: ['', Validators.maxLength(200)],
      institution: ['', [Validators.required, Validators.maxLength(200)]],
      fieldOfStudy: ['', [Validators.required, Validators.maxLength(150)]],
      degreeLevel: ['', Validators.required],
      graduationYear: [new Date().getFullYear(), [Validators.required, Validators.min(2000), Validators.max(2100)]],
      gitHubUrl: ['', Validators.pattern(URL_RULE)],
      portfolioUrl: ['', Validators.pattern(URL_RULE)],
    },
    { validators: passwordsMatch },
  );

  companyForm = this.fb.nonNullable.group(
    {
      companyName: ['', [Validators.required, Validators.maxLength(200)]],
      contactName: ['', [Validators.required, Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email]],
      website: ['', Validators.pattern(URL_RULE)],
      password: ['', [Validators.required, Validators.pattern(PASSWORD_RULE)]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordsMatch },
  );

  selectAccountType(type: AccountType): void {
    this.accountType = type;
    this.error.set('');
  }

  /** True when a control is invalid and the user has interacted with it. */
  invalid(form: 'candidate' | 'company', name: string): boolean {
    const group: AbstractControl = form === 'candidate' ? this.candidateForm : this.companyForm;
    const c = group.get(name);
    return !!c && c.invalid && (c.touched || c.dirty);
  }

  submitCandidate(): void {
    const f = this.candidateForm;
    if (f.invalid) {
      f.markAllAsTouched();
      this.error.set('Please fix the highlighted fields.');
      return;
    }
    const v = f.getRawValue();
    this.send({
      email: v.email.trim(),
      password: v.password,
      role: 'Candidate',
      candidate: {
        fullName: v.fullName.trim(),
        headline: v.headline.trim() || null,
        institution: v.institution.trim(),
        fieldOfStudy: v.fieldOfStudy.trim(),
        degreeLevel: v.degreeLevel,
        graduationYear: Number(v.graduationYear),
        gitHubUrl: v.gitHubUrl.trim() || null,
        portfolioUrl: v.portfolioUrl.trim() || null,
      },
    });
  }

  submitCompany(): void {
    const f = this.companyForm;
    if (f.invalid) {
      f.markAllAsTouched();
      this.error.set('Please fix the highlighted fields.');
      return;
    }
    const v = f.getRawValue();
    this.send({
      email: v.email.trim(),
      password: v.password,
      role: 'Employer',
      employer: {
        companyName: v.companyName.trim(),
        contactName: v.contactName.trim(),
        website: v.website.trim() || null,
      },
    });
  }

  private send(request: RegisterRequest): void {
    this.loading.set(true);
    this.error.set('');

    this.auth.register(request).subscribe({
      next: res => this.router.navigateByUrl(this.auth.homeFor(res.user.role)),
      error: err => {
        this.loading.set(false);
        this.error.set(errorMessage(err, 'Registration failed.'));
      },
    });
  }
}
