import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  imports: [FormsModule, RouterLink],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})

export class Login {

  email = '';
  password = '';

  onSubmit(): void {
    console.log('Email:', this.email);
    console.log('Password:', this.password);
  }
}