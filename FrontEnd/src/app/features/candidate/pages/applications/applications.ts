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

  // The API has no applications endpoint yet, so there is nothing to list.
  // When it exists, load the candidate's applications here.
  applications: {
    id: number;
    jobTitle: string;
    company: string;
    location: string;
    jobType: string;
    appliedDate: string;
    status: string;
    match: number;
  }[] = [];


  get filteredApplications() {

    if (this.selectedStatus === 'All') {
      return this.applications;
    }

    return this.applications.filter(
      application => application.status === this.selectedStatus
    );

  }

}