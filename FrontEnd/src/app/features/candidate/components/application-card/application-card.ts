import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-application-card',
  imports: [],
  templateUrl: './application-card.html',
  styleUrl: './application-card.css'
})
export class ApplicationCard {

  @Input() application: any;

}