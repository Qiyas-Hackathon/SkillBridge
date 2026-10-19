import { Job, Skill } from '../models/api.models';

/**
 * Percentage (0-100) of the job's required skills that the candidate has.
 * Computed in the browser because the API does not return a match score.
 */
export function matchScore(job: Job, candidateSkills: Skill[]): number {
  const required = job.requiredSkills ?? [];
  if (required.length === 0) return 0;
  const owned = new Set(candidateSkills.map(s => s.id));
  const hits = required.filter(s => owned.has(s.id)).length;
  return Math.round((hits / required.length) * 100);
}

/** The flattened shape the job card component displays. */
export interface JobCardData {
  id: number;
  title: string;
  company: string;
  postedAt: string;
  skills: string[];
  match: number;
}

export function toJobCard(job: Job, candidateSkills: Skill[]): JobCardData {
  return {
    id: job.id,
    title: job.title,
    company: job.companyName,
    postedAt: job.createdAt,
    skills: job.requiredSkills.map(s => s.name),
    match: matchScore(job, candidateSkills),
  };
}
