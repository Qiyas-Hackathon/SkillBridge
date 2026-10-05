import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {

  isEditing = false;

  candidate = {
    fullName: 'Abebe Kebede',
    headline: 'Junior Full Stack Developer',
    location: 'Addis Ababa, Ethiopia',

    email: 'abebe@example.com',
    phone: '+251 911 123 456',

    summary:
      'Computer Science graduate with a strong interest in full-stack web development. Experienced in Angular, TypeScript, C#, .NET, SQL and Git. Passionate about building reliable and user-friendly software applications.',

    skills: [
      'Angular',
      'TypeScript',
      'C#',
      '.NET',
      'SQL',
      'Git',
      'HTML',
      'CSS'
    ],

    education: [
      {
        degree: 'Bachelor of Science in Computer Science',
        institution: 'Addis Ababa University',
        year: '2025'
      }
    ],

    experience: [
      {
        position: 'Junior Developer',
        company: 'Technology Company',
        period: '2025 - Present',
        description:
          'Developing and maintaining web applications using Angular, TypeScript, C# and .NET.'
      }
    ],

    github: 'https://github.com/',
    linkedin: 'https://linkedin.com/',
    portfolio: 'https://portfolio.example.com',

    resume: 'Abebe_Kebede_Resume.pdf'
  };


  editProfile() {
    this.isEditing = true;
  }


  cancelEdit() {
    this.isEditing = false;
  }


  saveProfile() {
    this.isEditing = false;

    console.log('Profile saved:', this.candidate);
  }

}