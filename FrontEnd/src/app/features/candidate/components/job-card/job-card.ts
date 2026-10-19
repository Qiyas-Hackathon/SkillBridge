import { DatePipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { JobCardData } from '../../../../core/utils/match';
import { SkillList } from '../skill-list/skill-list';

@Component({
  selector: 'app-job-card',
  imports: [SkillList, RouterLink, DatePipe],
  templateUrl: './job-card.html',
  styleUrl: './job-card.css'
})
export class JobCard {

  @Input({ required: true }) job!: JobCardData;

}
