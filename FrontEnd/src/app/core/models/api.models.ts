// Shapes returned/accepted by the SkillBridge API (camelCase JSON).

export type Role = 'Candidate' | 'Employer';

export interface User {
  id: number;
  email: string;
  role: Role;
}

export interface AuthResponse {
  accessToken: string;
  expiresAtUtc: string;
  user: User;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface CandidateRegistration {
  fullName: string;
  headline?: string | null;
  institution: string;
  fieldOfStudy: string;
  degreeLevel: string;
  graduationYear: number;
  gitHubUrl?: string | null;
  portfolioUrl?: string | null;
}

export interface EmployerRegistration {
  companyName: string;
  contactName: string;
  website?: string | null;
}

export interface RegisterRequest {
  email: string;
  password: string;
  role: Role;
  candidate?: CandidateRegistration | null;
  employer?: EmployerRegistration | null;
}

export interface Skill {
  id: number;
  name: string;
}

export interface Job {
  id: number;
  employerProfileId: number;
  title: string;
  description: string;
  createdAt: string;
  companyName: string;
  requiredSkills: Skill[];
}

export interface JobRequest {
  title: string;
  description: string;
  requiredSkillIds: number[];
}

export interface CandidateProfile {
  id: number;
  userId: number;
  fullName: string;
  headline: string | null;
  institution: string;
  fieldOfStudy: string | null;
  degreeLevel: string | null;
  graduationYear: number | null;
  gitHubUrl: string | null;
  portfolioUrl: string | null;
  skills: Skill[];
}

export interface CandidateProfileRequest {
  fullName: string;
  institution: string;
  headline?: string | null;
  fieldOfStudy?: string | null;
  degreeLevel?: string | null;
  graduationYear?: number | null;
  gitHubUrl?: string | null;
  portfolioUrl?: string | null;
}

export interface EmployerProfile {
  id: number;
  userId: number;
  companyName: string;
  description: string | null;
}

export interface EmployerProfileRequest {
  companyName: string;
  description?: string | null;
}
