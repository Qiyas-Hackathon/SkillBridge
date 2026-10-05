import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-candidate-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './candidate-layout.html',
  styleUrl: './candidate-layout.css'
})
export class CandidateLayout {

  candidateName = 'Abebe Kebede';

}