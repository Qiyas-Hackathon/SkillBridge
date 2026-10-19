import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, of, throwError } from 'rxjs';
import { API_URL } from '../api.config';
import { EmployerProfile, EmployerProfileRequest } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class EmployerService {
  private http = inject(HttpClient);
  private url = `${API_URL}/employers/me`;

  /** Emits null when the employer has no profile yet (404). */
  getMe(): Observable<EmployerProfile | null> {
    return this.http.get<EmployerProfile>(this.url).pipe(
      catchError((err: HttpErrorResponse) => (err.status === 404 ? of(null) : throwError(() => err))),
    );
  }

  create(request: EmployerProfileRequest): Observable<EmployerProfile> {
    return this.http.post<EmployerProfile>(this.url, request);
  }

  update(request: EmployerProfileRequest): Observable<EmployerProfile> {
    return this.http.put<EmployerProfile>(this.url, request);
  }
}
