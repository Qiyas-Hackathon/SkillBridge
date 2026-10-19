import { Job } from '../models/api.models';
import { matchScore } from './match';

const job = (ids: number[]): Job => ({
  id: 1,
  employerProfileId: 1,
  title: 'T',
  description: 'D',
  createdAt: '2026-10-01T00:00:00Z',
  companyName: 'Acme',
  requiredSkills: ids.map(id => ({ id, name: `s${id}` })),
});

describe('matchScore', () => {
  it('is the share of required skills the candidate has', () => {
    expect(matchScore(job([1, 2, 3, 4]), [{ id: 1, name: 'a' }, { id: 2, name: 'b' }])).toBe(50);
  });

  it('is 0 when the job lists no skills', () => {
    expect(matchScore(job([]), [{ id: 1, name: 'a' }])).toBe(0);
  });

  it('is 100 when every skill is covered', () => {
    expect(matchScore(job([1]), [{ id: 1, name: 'a' }])).toBe(100);
  });
});
