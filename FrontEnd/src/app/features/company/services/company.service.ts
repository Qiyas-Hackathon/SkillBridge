import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Applicant, ApplicationStatus } from '../models/company.model';

/**
 * Applicant data. The backend has no applications endpoints yet, so every call
 * returns "nothing". Replace these with HttpClient calls when the API adds them.
 * (Jobs and the company profile already use the real API: JobsService / EmployerService.)
 */
@Injectable({ providedIn: 'root' })
export class CompanyService {
  getApplicants(_jobId?: number): Observable<Applicant[]> {
    return of([]);
  }

  getApplicant(_id: number): Observable<Applicant | undefined> {
    return of(undefined);
  }

  updateApplicantStatus(_id: number, _status: ApplicationStatus): Observable<Applicant | undefined> {
    return of(undefined);
  }
}
