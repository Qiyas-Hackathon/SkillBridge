import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, of, throwError } from 'rxjs';
import { API_URL } from '../api.config';
import { CandidateProfile, CandidateProfileRequest } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private http = inject(HttpClient);
  private url = `${API_URL}/candidates/me`;

  /** Emits null when the candidate has no profile yet (404). */
  getMe(): Observable<CandidateProfile | null> {
    return this.http.get<CandidateProfile>(this.url).pipe(
      catchError((err: HttpErrorResponse) => (err.status === 404 ? of(null) : throwError(() => err))),
    );
  }

  create(request: CandidateProfileRequest): Observable<CandidateProfile> {
    return this.http.post<CandidateProfile>(this.url, request);
  }

  update(request: CandidateProfileRequest): Observable<CandidateProfile> {
    return this.http.put<CandidateProfile>(this.url, request);
  }

  addSkill(skillId: number): Observable<void> {
    return this.http.post<void>(`${this.url}/skills/${skillId}`, {});
  }

  removeSkill(skillId: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/skills/${skillId}`);
  }
}
