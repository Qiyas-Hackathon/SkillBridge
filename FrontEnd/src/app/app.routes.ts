import { Routes } from '@angular/router';

export const routes: Routes = [

  {
    path: 'candidate',

    loadComponent: () =>
      import('./layouts/candidate-layout/candidate-layout')
        .then(m => m.CandidateLayout),

    children: [

      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/candidate/pages/dashboard/dashboard')
            .then(m => m.Dashboard)
      },

      {
        path: 'jobs',
        loadComponent: () =>
          import('./features/candidate/pages/job-search/job-search')
            .then(m => m.JobSearch)
      },

      {
        path: 'jobs/:id',
        loadComponent: () =>
          import('./features/candidate/pages/job-details/job-details')
            .then(m => m.JobDetails)
      },

      {
        path: 'jobs/:id/apply',
        loadComponent: () =>
          import('./features/candidate/pages/apply/apply')
            .then(m => m.Apply)
      },

      {
        path: 'applications',
        loadComponent: () =>
          import('./features/candidate/pages/applications/applications')
            .then(m => m.Applications)
      },

      {
        path: 'applications/:id',
        loadComponent: () =>
          import('./features/candidate/pages/application-details/application-details')
            .then(m => m.ApplicationDetails)
      },

      {
        path: 'profile',
        loadComponent: () =>
          import('./features/candidate/pages/profile/profile')
            .then(m => m.Profile)
      },

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }

    ]
  },


  {
    path: '',
    redirectTo: 'candidate/dashboard',
    pathMatch: 'full'
  },


  {
    path: '**',
    redirectTo: 'candidate/dashboard'
  }

];