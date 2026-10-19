import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CandidateProfile, CandidateProfileRequest, Skill } from '../../../../core/models/api.models';
import { CandidateService } from '../../../../core/services/candidate.service';
import { SkillsService } from '../../../../core/services/skills.service';
import { errorMessage } from '../../../../core/utils/errors';

const EMPTY_FORM: CandidateProfileRequest = {
  fullName: '',
  institution: '',
  headline: '',
  fieldOfStudy: '',
  degreeLevel: '',
  graduationYear: new Date().getFullYear(),
  gitHubUrl: '',
  portfolioUrl: '',
};

@Component({
  selector: 'app-profile',
  imports: [FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  private candidates = inject(CandidateService);
  private skillsApi = inject(SkillsService);

  loading = signal(true);
  saving = signal(false);
  error = signal('');
  isEditing = signal(false);

  /** null means the candidate has no profile yet and must create one. */
  candidate = signal<CandidateProfile | null>(null);
  catalogue = signal<Skill[]>([]);
  form: CandidateProfileRequest = { ...EMPTY_FORM };
  selectedSkillId: number | null = null;

  /** Catalogue skills the candidate does not have yet. */
  availableSkills = computed(() => {
    const owned = new Set((this.candidate()?.skills ?? []).map(s => s.id));
    return this.catalogue().filter(s => !owned.has(s.id));
  });

  constructor() {
    this.candidates.getMe().subscribe({
      next: profile => {
        this.candidate.set(profile);
        if (!profile) this.editProfile();   // first visit: go straight to the form
        this.loading.set(false);
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not load your profile.'));
        this.loading.set(false);
      },
    });

    this.skillsApi.getAll().subscribe({
      next: skills => this.catalogue.set(skills),
      error: () => { /* skills can still be removed; adding is just unavailable */ },
    });
  }

  editProfile() {
    const c = this.candidate();
    this.form = c
      ? {
          fullName: c.fullName,
          institution: c.institution,
          headline: c.headline ?? '',
          fieldOfStudy: c.fieldOfStudy ?? '',
          degreeLevel: c.degreeLevel ?? '',
          graduationYear: c.graduationYear,
          gitHubUrl: c.gitHubUrl ?? '',
          portfolioUrl: c.portfolioUrl ?? '',
        }
      : { ...EMPTY_FORM };
    this.error.set('');
    this.isEditing.set(true);
  }

  cancelEdit() {
    this.error.set('');
    this.isEditing.set(false);
  }

  saveProfile() {
    const f = this.form;
    if (!f.fullName?.trim() || !f.institution?.trim()) {
      this.error.set('Full name and institution are required.');
      return;
    }

    const body: CandidateProfileRequest = {
      fullName: f.fullName.trim(),
      institution: f.institution.trim(),
      headline: f.headline?.trim() || null,
      fieldOfStudy: f.fieldOfStudy?.trim() || null,
      degreeLevel: f.degreeLevel?.trim() || null,
      graduationYear: f.graduationYear ? Number(f.graduationYear) : null,
      gitHubUrl: f.gitHubUrl?.trim() || null,
      portfolioUrl: f.portfolioUrl?.trim() || null,
    };

    this.saving.set(true);
    this.error.set('');

    const request = this.candidate()
      ? this.candidates.update(body)
      : this.candidates.create(body);

    request.subscribe({
      next: profile => {
        this.candidate.set(profile);
        this.isEditing.set(false);
        this.saving.set(false);
      },
      error: err => {
        this.error.set(errorMessage(err, 'Could not save your profile.'));
        this.saving.set(false);
      },
    });
  }

  addSkill() {
    const skill = this.catalogue().find(s => s.id === this.selectedSkillId);
    const current = this.candidate();
    if (!skill || !current) return;

    this.candidates.addSkill(skill.id).subscribe({
      next: () => {
        this.candidate.set({ ...current, skills: [...current.skills, skill] });
        this.selectedSkillId = null;
      },
      error: err => this.error.set(errorMessage(err, 'Could not add that skill.')),
    });
  }

  removeSkill(skill: Skill) {
    const current = this.candidate();
    if (!current) return;

    this.candidates.removeSkill(skill.id).subscribe({
      next: () =>
        this.candidate.set({ ...current, skills: current.skills.filter(s => s.id !== skill.id) }),
      error: err => this.error.set(errorMessage(err, 'Could not remove that skill.')),
    });
  }
}
