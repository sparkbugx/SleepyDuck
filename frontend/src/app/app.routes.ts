import { Routes } from '@angular/router';

import { authGuard, signedOutGuard } from './core/services/auth.service';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    title: 'Sign in · SleepyDuck',
    canActivate: [signedOutGuard],
    loadComponent: () => import('./features/login/login').then((m) => m.Login),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./core/layout/shell').then((m) => m.Shell),
    children: [
      {
        path: 'dashboard',
        title: 'SleepyDuck',
        loadComponent: () => import('./features/dashboard/dashboard').then((m) => m.Dashboard),
      },
      {
        path: 'study',
        title: 'Study · SleepyDuck',
        loadComponent: () => import('./features/study/study-session').then((m) => m.StudySession),
      },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
