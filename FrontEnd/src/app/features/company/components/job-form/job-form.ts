import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Job, JobRequest, Skill } from '../../../../core/models/api.models';
import { SkillsService } from '../../../../core/services/skills.service';
import { errorMessage } from '../../../../core/utils/errors';

@Component({
  selector: 'app-job-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './job-form.html',
  styleUrl: './job-form.css',
})
export class JobForm {
  private fb = inject(FormBuilder);
  private skillsApi = inject(SkillsService);

  editing = false;
  catalogue = signal<Skill[]>([]);
  selected = signal<Set<number>>(new Set());
  skillsTouched = signal(false);
  newSkill = '';
  skillError = signal('');

  @Input() saving = false;
  @Input() error = '';

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(5000)]],
  });

  @Input() set job(job: Job | null | undefined) {
    if (!job) return;
    this.editing = true;
    this.form.patchValue({ title: job.title, description: job.description });
    this.selected.set(new Set(job.requiredSkills.map(s => s.id)));
  }

  @Output() submitted = new EventEmitter<JobRequest>();

  constructor() {
    this.skillsApi.getAll().subscribe({
      next: skills => this.catalogue.set(skills),
      error: err => this.skillError.set(errorMessage(err, 'Could not load skills.')),
    });
  }

  invalid(name: 'title' | 'description'): boolean {
    const c = this.form.controls[name];
    return c.invalid && (c.touched || c.dirty);
  }

  isSelected(id: number): boolean {
    return this.selected().has(id);
  }

  toggleSkill(id: number): void {
    const next = new Set(this.selected());
    next.has(id) ? next.delete(id) : next.add(id);
    this.selected.set(next);
    this.skillsTouched.set(true);
  }

  /** Adds a brand-new skill to the shared catalogue and selects it. */
  addNewSkill(): void {
    const name = this.newSkill.trim();
    if (!name) return;

    this.skillError.set('');
    const existing = this.catalogue().find(s => s.name.toLowerCase() === name.toLowerCase());
    if (existing) {
      if (!this.isSelected(existing.id)) this.toggleSkill(existing.id);
      this.newSkill = '';
      return;
    }

    this.skillsApi.create(name).subscribe({
      next: skill => {
        this.catalogue.update(list => [...list, skill]);
        this.toggleSkill(skill.id);
        this.newSkill = '';
      },
      error: err => this.skillError.set(errorMessage(err, 'Could not add that skill.')),
    });
  }

  submit() {
    this.skillsTouched.set(true);
    if (this.form.invalid || this.selected().size === 0) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    this.submitted.emit({
      title: v.title.trim(),
      description: v.description.trim(),
      requiredSkillIds: [...this.selected()],
    });
  }
}
