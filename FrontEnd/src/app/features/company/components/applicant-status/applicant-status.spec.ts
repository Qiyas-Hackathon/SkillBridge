import { ComponentFixture, TestBed } from '@angular/core/testing';
import { vi } from 'vitest';
import { ApplicantStatus } from './applicant-status';

describe('ApplicantStatus', () => {
  let fixture: ComponentFixture<ApplicantStatus>;
  let el: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({ imports: [ApplicantStatus] }).compileComponents();
    fixture = TestBed.createComponent(ApplicantStatus);
    el = fixture.nativeElement;
  });

  it('shows a badge when not editable', () => {
    fixture.componentRef.setInput('status', 'hired');
    fixture.detectChanges();
    expect(el.querySelector('.badge')?.textContent).toContain('hired');
    expect(el.querySelector('select')).toBeNull();
  });

  it('shows a select when editable', () => {
    fixture.componentRef.setInput('editable', true);
    fixture.detectChanges();
    expect(el.querySelector('select')).not.toBeNull();
    expect(el.querySelectorAll('option').length).toBe(5);
  });

  it('emits the new status on change', () => {
    fixture.componentRef.setInput('editable', true);
    fixture.detectChanges();
    const spy = vi.fn();
    fixture.componentInstance.changed.subscribe(spy);

    const select = el.querySelector('select') as HTMLSelectElement;
    select.value = 'rejected';
    select.dispatchEvent(new Event('change'));

    expect(spy).toHaveBeenCalledWith('rejected');
  });
});