import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router } from '@angular/router';

/**
 * Single-user sign-in. There is no auth backend yet, so every sign-in path
 * goes straight to the dashboard.
 */
@Component({
  selector: 'app-login',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login.html',
})
export class Login {
  private readonly router = inject(Router);

  login(event?: Event): void {
    event?.preventDefault();
    void this.router.navigate(['/dashboard']);
  }
}
