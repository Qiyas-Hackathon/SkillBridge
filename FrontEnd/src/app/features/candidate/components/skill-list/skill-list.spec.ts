import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { SkillList } from './skill-list';

describe('SkillList', () => {
  let fixture: ComponentFixture<SkillList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkillList],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(SkillList);
    
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});
