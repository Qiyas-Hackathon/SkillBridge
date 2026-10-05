import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-apply',
  imports: [FormsModule],
  templateUrl: './apply.html',
  styleUrl: './apply.css'
})
export class Apply {

  job = {
    id: 1,
    title: 'Junior Angular Developer',
    company: 'ABC Technology',
    location: 'Addis Ababa'
  };

  application = {
    fullName: 'Abebe Kebede',
    email: 'abebe@example.com',
    phone: '',
    coverLetter: '',
    portfolioUrl: '',
    resume: ''
  };

  submitted = false;

  submitApplication() {
    this.submitted = true;

    console.log(
      'Application submitted:',
      this.application
    );
  }

}