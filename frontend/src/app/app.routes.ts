import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    title: 'SleepyDuck',
    loadComponent: () => import('./features/dashboard/dashboard').then((m) => m.Dashboard),
  },
  {
    path: 'study',
    title: 'Study · SleepyDuck',
    loadComponent: () => import('./features/study/study-session').then((m) => m.StudySession),
  },
  { path: '**', redirectTo: '' },
];
