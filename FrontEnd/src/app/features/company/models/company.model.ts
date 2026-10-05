export type JobStatus = 'open' | 'closed';
export type JobType = 'Full-time' | 'Part-time' | 'Internship' | 'Contract';

// TEMPORARY: replace with the shared type once the team agrees on one
export type ApplicationStatus = 'pending' | 'reviewed' | 'shortlisted' | 'rejected' | 'hired';

export const APPLICATION_STATUSES: ApplicationStatus[] = [
  'pending', 'reviewed', 'shortlisted', 'rejected', 'hired',
];

export interface Job {
  id: number;
  title: string;
  description: string;
  location: string;
  type: JobType;
  skills: string[];
  salary?: string;
  deadline: string;
  status: JobStatus;
  postedAt: string;
}

export type JobInput = Omit<Job, 'id' | 'postedAt' | 'status'>;

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

export interface CompanyStats {
  activeJobs: number;
  totalApplicants: number;
  shortlisted: number;
  recent: Applicant[];
}