// Applications are not part of the API yet, so these types only describe the
// placeholder applicant screens. Jobs now use the shared API types in
// core/models/api.models.ts.

export type ApplicationStatus = 'pending' | 'reviewed' | 'shortlisted' | 'rejected' | 'hired';

export const APPLICATION_STATUSES: ApplicationStatus[] = [
  'pending', 'reviewed', 'shortlisted', 'rejected', 'hired',
];

export interface Applicant {
  id: number;
  jobId: number;
  name: string;
  email: string;
  skills: string[];
  matchScore: number;
  status: ApplicationStatus;
  appliedAt: string;
  experience: string;
  bio: string;
}
