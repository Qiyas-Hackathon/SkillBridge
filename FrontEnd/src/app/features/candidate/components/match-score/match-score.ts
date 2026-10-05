import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-match-score',
  imports: [],
  templateUrl: './match-score.html',
  styleUrl: './match-score.css'
})
export class MatchScore {

  @Input() score = 0;

}