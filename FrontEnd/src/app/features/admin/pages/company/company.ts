import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface AdminCompany {
  id: number;
  companyName: string;
  contactName: string;
  email: string;
  website: string;
  status: 'Pending' | 'Approved';
}

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './company.html',
  styleUrl: './company.css'
})
export class Company {

  companies: AdminCompany[] = [
    {
      id: 1,
      companyName: 'ABC Technology',
      contactName: 'John Doe',
      email: 'john@abctech.com',
      website: 'https://abctech.com',
      status: 'Pending'
    },
    {
      id: 2,
      companyName: 'XYZ Solutions',
      contactName: 'Sara Ahmed',
      email: 'sara@xyzsolutions.com',
      website: 'https://xyzsolutions.com',
      status: 'Approved'
    },
    {
      id: 3,
      companyName: 'NextGen Software',
      contactName: 'Michael Smith',
      email: 'michael@nextgen.com',
      website: 'https://nextgen.com',
      status: 'Pending'
    }
  ];

  approveCompany(company: AdminCompany): void {
    company.status = 'Approved';
  }
}