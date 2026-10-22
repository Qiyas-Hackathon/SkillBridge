import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Applicant, ApplicationStatus } from '../models/company.model';


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
