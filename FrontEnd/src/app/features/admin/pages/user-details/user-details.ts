import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface AdminUser {
  id: number;
  email: string;
  role: 'Candidate' | 'Employer';
}

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-details.html',
  styleUrl: './user-details.css'
})
export class UserDetails {

  userId = '';

  user: AdminUser | null = null;

  // Temporary mock data.
  // This will be replaced with the backend API later.
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

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id') ?? '';

    const id = Number(this.userId);

    this.user = this.users.find(user => user.id === id) ?? null;
  }
}