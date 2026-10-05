import { Component } from '@angular/core';
import { JobCard } from '../../components/job-card/job-card';

@Component({
  selector: 'app-dashboard',
  imports: [JobCard],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  candidateName = 'Abebe Kebede';

  stats = {
    applications: 12,
    pending: 5,
    shortlisted: 3
  };

  recommendedJobs = [
    {
      id: 1,
      title: 'Junior Angular Developer',
      company: 'ABC Technology',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['Angular', 'TypeScript', 'C#'],
      match: 87
    },
    {
      id: 2,
      title: 'Junior Full Stack Developer',
      company: 'Tech Solutions Ethiopia',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['C#', '.NET', 'Angular', 'SQL'],
      match: 82
    },
    {
      id: 3,
      title: 'Frontend Developer Intern',
      company: 'Digital Solutions',
      location: 'Remote',
      jobType: 'Internship',
      skills: ['Angular', 'TypeScript', 'HTML', 'CSS'],
      match: 78
    }
  ];

}