import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    title: 'Sign in · SleepyDuck',
    loadComponent: () => import('./features/login/login').then((m) => m.Login),
  },
  {
    path: '',
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
