import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="mx-auto flex min-h-dvh max-w-4xl flex-col px-4 sm:px-6">
      <header class="flex items-center justify-between py-5">
        <a routerLink="/" class="group flex items-center gap-2.5 rounded-lg">
          <img
            src="favicon.svg"
            alt=""
            class="size-7 rounded-lg transition-transform duration-300 group-hover:-rotate-6"
          />
          <span class="text-[15px] font-semibold tracking-tight text-white">SleepyDuck</span>
        </a>
        <span class="hidden text-xs text-slate-500 sm:block">Microsoft 365 Admin Troubleshooting</span>
      </header>

      <main class="flex-1 pb-16 pt-4">
        <router-outlet />
      </main>
    </div>
  `,
})
export class App {}
