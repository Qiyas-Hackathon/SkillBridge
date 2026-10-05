import { Component } from '@angular/core';
import { SkillList } from '../../components/skill-list/skill-list';
import { MatchScore } from '../../components/match-score/match-score';

@Component({
  selector: 'app-job-details',
  imports: [SkillList, MatchScore],
  templateUrl: './job-details.html',
  styleUrl: './job-details.css'
})
export class JobDetails {

  job = {
    id: 1,
    title: 'Junior Angular Developer',
    company: 'ABC Technology',
    location: 'Addis Ababa',
    jobType: 'Full-time',
    salary: 'ETB 15,000 - 25,000',
    postedDate: '2 days ago',
    match: 92,

    skills: [
      'Angular',
      'TypeScript',
      'HTML',
      'CSS',
      'Git'
    ],

    description:
      'We are looking for a motivated Junior Angular Developer to join our development team. You will work with experienced developers to build modern web applications.',

    responsibilities: [
      'Develop responsive web applications using Angular',
      'Write clean and maintainable TypeScript code',
      'Work with backend developers to integrate APIs',
      'Fix bugs and improve application performance',
      'Participate in code reviews and team meetings'
    ],

    qualifications: [
      'Bachelor degree in Computer Science or related field',
      'Basic knowledge of Angular and TypeScript',
      'Understanding of HTML and CSS',
      'Knowledge of Git and version control',
      'Strong problem-solving skills'
    ]
  };

}