import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
@Component({
  imports: [RouterLink],
  selector: 'app-dashboard',
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})

export class Dashboard {

  totalUsers = 0;
  totalCandidates = 0;
  totalEmployers = 0;
  totalJobs = 0;

}
