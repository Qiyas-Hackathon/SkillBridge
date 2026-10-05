
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/public/pages/home/home')
        .then(m => m.Home)
  },

  {
    path: 'login',
    loadComponent: () =>
      import('./features/public/pages/login/login')
        .then(m => m.Login)
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./features/public/pages/register/register')
        .then(m => m.Register)
  },

  {
    path: 'admin',
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/admin/pages/dashboard/dashboard')
            .then(m => m.Dashboard)
      },

      {
        path: 'users',
        loadComponent: () =>
          import('./features/admin/pages/users/users')
            .then(m => m.Users)
      },
 {    path: 'users/:id',
      loadComponent: () =>
        import('./features/admin/pages/user-details/user-details')
          .then(m => m.UserDetails)
    },
      {
        path: 'companies',
        loadComponent: () =>
          import('./features/admin/pages/company/company')
            .then(m => m.Company)
      }
    ]
  },

  {
    path: '**',
    redirectTo: ''
  }
];
