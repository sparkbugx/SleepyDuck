import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

/** Single-account sign-in. The API checks the email and password against its .env. */
@Component({
  selector: 'app-login',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.html',
})
export class Login {
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);

  protected readonly pending = signal(false);
  protected readonly error = signal<string | null>(null);

  login(event: Event, email: string, password: string): void {
    event.preventDefault();
    if (this.pending()) {
      return;
    }
    if (!email.trim() || !password) {
      this.error.set('Enter your email and password.');
      return;
    }

    this.pending.set(true);
    this.error.set(null);
    this.auth.login(email.trim(), password).subscribe({
      next: () => void this.router.navigate(['/dashboard']),
      error: (error: unknown) => {
        this.pending.set(false);
        this.error.set(messageFor(error));
      },
    });
  }
}

function messageFor(error: unknown): string {
  const status = error instanceof HttpErrorResponse ? error.status : 0;
  if (status === 401) {
    return 'Wrong email or password.';
  }
  if (status === 429) {
    return 'Too many attempts. Wait a minute and try again.';
  }
  return "Couldn't reach the server. Try again.";
}
