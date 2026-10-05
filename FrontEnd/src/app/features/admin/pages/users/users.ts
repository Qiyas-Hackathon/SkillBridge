import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface AdminUser {
  id: number;
  email: string;
  role: 'Candidate' | 'Employer';
}

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css'
})
export class Users {

  searchTerm = '';
  selectedRole = 'All';

  users: AdminUser[] = [
    {
      id: 1,
      email: 'candidate@example.com',
      role: 'Candidate'
    },
    {
      id: 2,
      email: 'company@example.com',
      role: 'Employer'
    }
  ];

  get filteredUsers(): AdminUser[] {
    return this.users.filter(user => {

      const matchesSearch =
        user.email
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase());

      const matchesRole =
        this.selectedRole === 'All' ||
        user.role === this.selectedRole;

      return matchesSearch && matchesRole;
    });
  }
}