import { HttpClient } from '@angular/common/http';
import { Injectable, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { API_URL } from '../api.config';
import { AuthResponse, LoginRequest, RegisterRequest, Role, User } from '../models/api.models';

interface Session {
  accessToken: string;
  expiresAtUtc: string;
  user: User;
}

const STORAGE_KEY = 'skillbridge.session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  private session = signal<Session | null>(this.load());

  readonly user = computed(() => this.session()?.user ?? null);
  readonly token = computed(() => this.session()?.accessToken ?? null);
  readonly role = computed<Role | null>(() => this.session()?.user.role ?? null);
  readonly isLoggedIn = computed(() => this.session() !== null);

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/auth/login`, request)
      .pipe(tap(res => this.save(res)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${API_URL}/auth/register`, request)
      .pipe(tap(res => this.save(res)));
  }

  logout(redirect = true): void {
    this.session.set(null);
    if (this.isBrowser) {
      try { localStorage.removeItem(STORAGE_KEY); } catch { /* storage unavailable */ }
    }
    if (redirect) this.router.navigate(['/login']);
  }

  /** Landing page for a role after sign-in. */
  homeFor(role: Role | null): string {
    return role === 'Employer' ? '/company/dashboard' : '/candidate/dashboard';
  }

  private save(res: AuthResponse): void {
    const session: Session = {
      accessToken: res.accessToken,
      expiresAtUtc: res.expiresAtUtc,
      user: res.user,
    };
    this.session.set(session);
    if (this.isBrowser) {
      try { localStorage.setItem(STORAGE_KEY, JSON.stringify(session)); } catch { /* ignore */ }
    }
  }

  private load(): Session | null {
    if (!this.isBrowser) return null;
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) return null;
      const session = JSON.parse(raw) as Session;
      if (!session.accessToken || !session.user) return null;

      // The API sends a UTC DateTime; treat a missing "Z" as UTC.
      const iso = session.expiresAtUtc.endsWith('Z') ? session.expiresAtUtc : session.expiresAtUtc + 'Z';
      if (new Date(iso).getTime() <= Date.now()) {
        localStorage.removeItem(STORAGE_KEY);
        return null;
      }
      return session;
    } catch {
      return null;
    }
  }
}
