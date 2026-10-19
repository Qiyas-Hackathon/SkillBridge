import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../api.config';
import { Job, JobRequest } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class JobsService {
  private http = inject(HttpClient);

  getAll(filters: { search?: string; skillId?: number | null } = {}): Observable<Job[]> {
    let params = new HttpParams();
    if (filters.search?.trim()) params = params.set('search', filters.search.trim());
    if (filters.skillId) params = params.set('skillId', filters.skillId);
    return this.http.get<Job[]>(`${API_URL}/jobs`, { params });
  }

  get(id: number): Observable<Job> {
    return this.http.get<Job>(`${API_URL}/jobs/${id}`);
  }

  create(request: JobRequest): Observable<Job> {
    return this.http.post<Job>(`${API_URL}/jobs`, request);
  }

  update(id: number, request: JobRequest): Observable<Job> {
    return this.http.put<Job>(`${API_URL}/jobs/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${API_URL}/jobs/${id}`);
  }
}
