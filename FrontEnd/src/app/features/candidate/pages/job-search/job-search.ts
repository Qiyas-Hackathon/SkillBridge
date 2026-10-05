import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { JobCard } from '../../components/job-card/job-card';

@Component({
  selector: 'app-job-search',
  imports: [FormsModule, JobCard],
  templateUrl: './job-search.html',
  styleUrl: './job-search.css'
})
export class JobSearch {

  searchTerm = '';
  selectedLocation = 'All Locations';
  selectedJobType = 'All Types';

  jobs = [
    {
      id: 1,
      title: 'Junior Angular Developer',
      company: 'ABC Technology',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['Angular', 'TypeScript', 'HTML', 'CSS'],
      match: 92
    },
    {
      id: 2,
      title: 'Junior Full Stack Developer',
      company: 'Tech Solutions Ethiopia',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['C#', '.NET', 'Angular', 'SQL'],
      match: 87
    },
    {
      id: 3,
      title: 'Frontend Developer Intern',
      company: 'Digital Solutions',
      location: 'Remote',
      jobType: 'Internship',
      skills: ['Angular', 'TypeScript', 'JavaScript'],
      match: 84
    },
    {
      id: 4,
      title: 'Software Developer',
      company: 'Ethio Digital',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['C#', '.NET', 'SQL', 'Git'],
      match: 79
    },
    {
      id: 5,
      title: 'Web Developer Intern',
      company: 'Innovation Hub',
      location: 'Remote',
      jobType: 'Internship',
      skills: ['HTML', 'CSS', 'JavaScript'],
      match: 76
    },
    {
      id: 6,
      title: 'TypeScript Developer',
      company: 'Tech Africa',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      skills: ['TypeScript', 'Angular', 'Git'],
      match: 74
    }
  ];

  get filteredJobs() {
    return this.jobs.filter(job => {

      const matchesSearch =
        job.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.company.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.skills.some(skill =>
          skill.toLowerCase().includes(this.searchTerm.toLowerCase())
        );

      const matchesLocation =
        this.selectedLocation === 'All Locations' ||
        job.location === this.selectedLocation;

      const matchesJobType =
        this.selectedJobType === 'All Types' ||
        job.jobType === this.selectedJobType;

      return matchesSearch && matchesLocation && matchesJobType;
    });
  }
}