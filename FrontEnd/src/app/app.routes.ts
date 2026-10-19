import { Routes } from '@angular/router';
import { guestGuard, roleGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  // ========================= PUBLIC =========================
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./features/public/pages/home/home').then(m => m.Home),
  },
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/public/pages/login/login').then(m => m.Login),
  },
  {
    path: 'register',
    canActivate: [guestGuard],
    loadComponent: () => import('./features/public/pages/register/register').then(m => m.Register),
  },

  // ========================= CANDIDATE =========================
  {
    path: 'candidate',
    canActivate: [roleGuard('Candidate')],
    loadComponent: () =>
      import('./layouts/candidate-layout/candidate-layout').then(m => m.CandidateLayout),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/candidate/pages/dashboard/dashboard').then(m => m.Dashboard),
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./features/candidate/pages/profile/profile').then(m => m.Profile),
      },
      {
        path: 'jobs',
        loadComponent: () =>
          import('./features/candidate/pages/job-search/job-search').then(m => m.JobSearch),
      },
      {
        path: 'jobs/:id',
        loadComponent: () =>
          import('./features/candidate/pages/job-details/job-details').then(m => m.JobDetails),
      },
      {
        path: 'jobs/:id/apply',
        loadComponent: () =>
          import('./features/candidate/pages/apply/apply').then(m => m.Apply),
      },
      {
        path: 'applications',
        loadComponent: () =>
          import('./features/candidate/pages/applications/applications').then(m => m.Applications),
      },
    ],
  },

  // ========================= COMPANY (Employer) =========================
  {
    path: 'company',
    canActivate: [roleGuard('Employer')],
    loadComponent: () =>
      import('./layouts/company-layout/company-layout').then(m => m.CompanyLayout),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/company/pages/dashboard/dashboard').then(m => m.Dashboard),
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./features/company/pages/profile/profile').then(m => m.CompanyProfile),
      },
      {
        path: 'jobs',
        loadComponent: () =>
          import('./features/company/pages/manage-jobs/manage-jobs').then(m => m.ManageJobs),
      },
      {
        path: 'jobs/create',
        loadComponent: () =>
          import('./features/company/pages/post-job/post-job').then(m => m.PostJob),
      },
      {
        path: 'jobs/:id/edit',
        loadComponent: () =>
          import('./features/company/pages/post-job/post-job').then(m => m.PostJob),
      },
      {
        path: 'jobs/:jobId/applicants',
        loadComponent: () =>
          import('./features/company/pages/applicants/applicants').then(m => m.Applicants),
      },
      {
        path: 'applicants',
        loadComponent: () =>
          import('./features/company/pages/applicants/applicants').then(m => m.Applicants),
      },
      {
        path: 'applicants/:id',
        loadComponent: () =>
          import('./features/company/pages/applicant-details/applicant-details').then(
            m => m.ApplicantDetails,
          ),
      },
    ],
  },

  // ========================= ADMIN (UI only - no API yet) =========================
  {
    path: 'admin',
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/admin/pages/dashboard/dashboard').then(m => m.Dashboard),
      },
      {
        path: 'users',
        loadComponent: () => import('./features/admin/pages/users/users').then(m => m.Users),
      },
      {
        path: 'users/:id',
        loadComponent: () =>
          import('./features/admin/pages/user-details/user-details').then(m => m.UserDetails),
      },
      {
        path: 'companies',
        loadComponent: () => import('./features/admin/pages/company/company').then(m => m.Company),
      },
    ],
  },

  { path: '**', redirectTo: '' },
];
