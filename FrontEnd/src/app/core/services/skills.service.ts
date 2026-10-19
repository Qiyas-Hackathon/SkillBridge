import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL } from '../api.config';
import { Skill } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class SkillsService {
  private http = inject(HttpClient);

  getAll(): Observable<Skill[]> {
    return this.http.get<Skill[]>(`${API_URL}/skills`);
  }

  /** Any signed-in user may add a skill to the shared catalogue. */
  create(name: string): Observable<Skill> {
    return this.http.post<Skill>(`${API_URL}/skills`, { name });
  }
}
