import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { API_URL } from '../api.config';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let auth: AuthService;
  let http: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    });
    auth = TestBed.inject(AuthService);
    http = TestBed.inject(HttpTestingController);
  });

  it('stores the session after login and clears it on logout', () => {
    auth.login({ email: 'a@b.c', password: 'Passw0rd' }).subscribe();

    http.expectOne(`${API_URL}/auth/login`).flush({
      accessToken: 'jwt-token',
      expiresAtUtc: new Date(Date.now() + 3600_000).toISOString(),
      user: { id: 1, email: 'a@b.c', role: 'Employer' },
    });

    expect(auth.isLoggedIn()).toBe(true);
    expect(auth.token()).toBe('jwt-token');
    expect(auth.role()).toBe('Employer');
    expect(auth.homeFor(auth.role())).toBe('/company/dashboard');

    auth.logout(false);
    expect(auth.isLoggedIn()).toBe(false);
    expect(localStorage.getItem('skillbridge.session')).toBeNull();
  });
});
