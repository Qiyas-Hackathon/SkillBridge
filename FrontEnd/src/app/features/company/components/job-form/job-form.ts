import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Job, JobInput, JobType } from '../../models/company.model';

@Component({
  selector: 'app-job-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './job-form.html',
  styleUrl: './job-form.css',
})
export class JobForm {
  private fb = inject(FormBuilder);

  types: JobType[] = ['Full-time', 'Part-time', 'Internship', 'Contract'];
  editing = false;

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    description: ['', [Validators.required, Validators.minLength(20)]],
    location: ['', Validators.required],
    type: ['Full-time' as JobType, Validators.required],
    skills: ['', Validators.required],
    salary: [''],
    deadline: ['', Validators.required],
  });

  @Input() set job(job: Job | null | undefined) {
    if (!job) return;
    this.editing = true;
    this.form.patchValue({
      title: job.title,
      description: job.description,
      location: job.location,
      type: job.type,
      skills: job.skills.join(', '),
      salary: job.salary ?? '',
      deadline: job.deadline,
    });
  }

  @Output() submitted = new EventEmitter<JobInput>();

  invalid(name: string): boolean {
    const c = this.form.get(name);
    return !!c && c.invalid && (c.touched || c.dirty);
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    this.submitted.emit({
      title: v.title.trim(),
      description: v.description.trim(),
      location: v.location.trim(),
      type: v.type,
      skills: v.skills.split(',').map(s => s.trim()).filter(Boolean),
      salary: v.salary.trim() || undefined,
      deadline: v.deadline,
    });
  }
}