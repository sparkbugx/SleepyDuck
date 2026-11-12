import { DOCUMENT, inject, Injectable, signal } from '@angular/core';

export type Theme = 'dark' | 'light';

const STORAGE_KEY = 'sleepyduck-theme';
const THEME_COLORS: Record<Theme, string> = { dark: '#121212', light: '#F4F4F0' };

/** Dark is the default. The script in index.html restores a saved choice before first paint. */
@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly document = inject(DOCUMENT);
  private readonly root = this.document.documentElement;

  readonly theme = signal<Theme>(this.root.classList.contains('dark') ? 'dark' : 'light');

  toggle(): void {
    const next: Theme = this.theme() === 'dark' ? 'light' : 'dark';
    const apply = () => this.apply(next);

    const reducedMotion = this.document.defaultView?.matchMedia('(prefers-reduced-motion: reduce)').matches;
    if (this.document.startViewTransition && !reducedMotion) {
      this.document.startViewTransition(apply);
    } else {
      apply();
    }

    try {
      localStorage.setItem(STORAGE_KEY, next);
    } catch {
      // Storage can be blocked. The theme still applies for this visit.
    }
  }

  private apply(theme: Theme): void {
    this.root.classList.toggle('dark', theme === 'dark');
    this.document.querySelector('meta[name="theme-color"]')?.setAttribute('content', THEME_COLORS[theme]);
    this.theme.set(theme);
  }
}
