
import { Component } from '@angular/core';

@Component({
  selector: 'app-application-details',
  imports: [],
  templateUrl: './application-details.html',
  styleUrl: './application-details.css'
})
export class ApplicationDetails {

  application = {

    id: 1,

    jobTitle: 'Junior Angular Developer',

    company: 'ABC Technology',

    location: 'Addis Ababa',

    jobType: 'Full-time',

    salary: 'ETB 15,000 - 25,000',

    appliedDate: 'October 1, 2026',

    status: 'Pending',

    match: 92,

    coverLetter:
      'Dear Hiring Manager,\n\nI am excited to apply for the Junior Angular Developer position at ABC Technology. I have a strong interest in frontend development and experience working with Angular, TypeScript, HTML, and CSS.\n\nI am eager to contribute to your development team and continue improving my technical skills.',

    resume:
      'Abebe_Kebede_Resume.pdf',

    portfolio:
      'https://portfolio.example.com',

    timeline: [

      {
        title: 'Application Submitted',
        date: 'October 1, 2026',
        description:
          'Your application was successfully submitted.'
      },

      {
        title: 'Application Under Review',
        date: 'October 2, 2026',
        description:
          'The employer is reviewing your application.'
      },

      {
        title: 'Interview',
        date: 'Not scheduled',
        description:
          'No interview has been scheduled yet.'
      }

    ]

  };


  withdrawApplication() {

    const confirmed = window.confirm(
      'Are you sure you want to withdraw this application?'
    );

    if (!confirmed) {
      return;
    }

    this.application.status = 'Withdrawn';

    this.application.timeline.push({

      title: 'Application Withdrawn',

      date: 'October 5, 2026',

      description:
        'You withdrew this application.'

    });

  }

}
