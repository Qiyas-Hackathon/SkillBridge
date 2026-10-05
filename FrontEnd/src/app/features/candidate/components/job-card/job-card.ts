import { Component, Input } from '@angular/core';
import { SkillList } from '../skill-list/skill-list';

@Component({
  selector: 'app-job-card',
  imports: [SkillList],
  templateUrl: './job-card.html',
  styleUrl: './job-card.css'
})
export class JobCard {

  @Input() job: any;

}