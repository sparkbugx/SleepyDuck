import { HttpClient, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, Observable, of, tap, throwError } from 'rxjs';

interface Session {
  email: string;
}

/** Cookie-based sign-in against the API. The session cookie is HttpOnly, so it is never read here. */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  /** Email of the signed-in account, or null when signed out or not checked yet. */
  readonly email = signal<string | null>(null);

  login(email: string, password: string): Observable<void> {
    return this.http.post<Session>('/api/auth/login', { email, password }).pipe(
      tap((session) => this.email.set(session.email)),
      map(() => undefined),
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>('/api/auth/logout', null).pipe(tap(() => this.email.set(null)));
  }

  /** Asks the API whether the session cookie is still valid. */
  isSignedIn(): Observable<boolean> {
    if (this.email() !== null) {
      return of(true);
    }
    return this.http.get<Session>('/api/auth/me').pipe(
      tap((session) => this.email.set(session.email)),
      map(() => true),
      catchError(() => of(false)),
    );
  }
}

/** Sends signed-out visitors to the login page. */
export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  return inject(AuthService)
    .isSignedIn()
    .pipe(map((signedIn) => signedIn || router.createUrlTree(['/login'])));
};

/** Skips the login page when the session is still valid. */
export const signedOutGuard: CanActivateFn = () => {
  const router = inject(Router);
  return inject(AuthService)
    .isSignedIn()
    .pipe(map((signedIn) => (signedIn ? router.createUrlTree(['/dashboard']) : true)));
};

/** When the session expires mid-use, any API call answers 401: go back to the login page. */
export const unauthorizedInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const auth = inject(AuthService);
  return next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && error.status === 401 && !req.url.startsWith('/api/auth/')) {
        auth.email.set(null);
        void router.navigate(['/login']);
      }
      return throwError(() => error);
    }),
  );
};
