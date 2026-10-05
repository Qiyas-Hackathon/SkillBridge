import { Component } from '@angular/core';
import { ApplicationCard } from '../../components/application-card/application-card';

@Component({
  selector: 'app-applications',
  imports: [ApplicationCard],
  templateUrl: './applications.html',
  styleUrl: './applications.css'
})
export class Applications {

  selectedStatus = 'All';

  applications = [

    {
      id: 1,
      jobTitle: 'Junior Angular Developer',
      company: 'ABC Technology',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      appliedDate: 'October 1, 2026',
      status: 'Pending',
      match: 92
    },

    {
      id: 2,
      jobTitle: 'Junior Full Stack Developer',
      company: 'Tech Solutions Ethiopia',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      appliedDate: 'September 28, 2026',
      status: 'Shortlisted',
      match: 87
    },

    {
      id: 3,
      jobTitle: 'Frontend Developer Intern',
      company: 'Digital Solutions',
      location: 'Remote',
      jobType: 'Internship',
      appliedDate: 'September 25, 2026',
      status: 'Rejected',
      match: 84
    },

    {
      id: 4,
      jobTitle: 'Software Developer',
      company: 'Ethio Digital',
      location: 'Addis Ababa',
      jobType: 'Full-time',
      appliedDate: 'September 20, 2026',
      status: 'Pending',
      match: 79
    }

  ];


  get filteredApplications() {

    if (this.selectedStatus === 'All') {
      return this.applications;
    }

    return this.applications.filter(
      application => application.status === this.selectedStatus
    );

  }

}