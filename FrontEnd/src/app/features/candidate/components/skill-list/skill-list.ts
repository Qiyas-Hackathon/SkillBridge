import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-skill-list',
  imports: [],
  templateUrl: './skill-list.html',
  styleUrl: './skill-list.css'
})
export class SkillList {

  @Input() skills: string[] = [];

}