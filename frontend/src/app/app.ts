import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

import { ThemeService } from './core/services/theme.service';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="mx-auto flex min-h-dvh max-w-3xl flex-col px-4 sm:px-6">
      <header class="rule flex items-center justify-between border-b py-4">
        <a routerLink="/" class="group flex items-center gap-2.5 rounded">
          <img
            src="favicon.svg"
            alt=""
            class="size-7 transition-transform duration-300 group-hover:-rotate-6"
          />
          <span class="text-strong font-display text-lg font-semibold tracking-tight">SleepyDuck</span>
        </a>

        <div class="flex items-center gap-4">
          <span class="text-soft hidden text-xs sm:block">Microsoft 365 Admin Troubleshooting</span>
          <button
            type="button"
            class="btn-secondary !size-9 !p-0"
            [attr.aria-label]="theme.theme() === 'dark' ? 'Switch to light mode' : 'Switch to dark mode'"
            (click)="theme.toggle()"
          >
            @if (theme.theme() === 'dark') {
              <svg class="size-[18px]" viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="1.700" stroke-linecap="round" aria-hidden="true">
                <circle cx="10" cy="10" r="3.500" />
                <path d="M10 2.500v1.500M10 16v1.500M2.500 10H4M16 10h1.500M4.700 4.700l1 1M14.300 14.300l1 1M4.700 15.300l1-1M14.300 5.700l1-1" />
              </svg>
            } @else {
              <svg class="size-[18px]" viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="1.700" stroke-linejoin="round" aria-hidden="true">
                <path d="M16.500 11.700A6.800 6.800 0 0 1 8.300 3.500a6.800 6.800 0 1 0 8.200 8.200Z" />
              </svg>
            }
          </button>
        </div>
      </header>

      <main class="flex-1 pb-16 pt-10">
        <router-outlet />
      </main>
    </div>
  `,
})
export class App {
  protected readonly theme = inject(ThemeService);
}
