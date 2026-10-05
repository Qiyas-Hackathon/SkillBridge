import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { PostJob } from './post-job';

function setup(params: Record<string, string>) {
  TestBed.configureTestingModule({
    imports: [PostJob],
    providers: [
      provideRouter([]),
      { provide: ActivatedRoute, useValue: { snapshot: { paramMap: convertToParamMap(params) } } },
    ],
  });
  const fixture: ComponentFixture<PostJob> = TestBed.createComponent(PostJob);
  fixture.detectChanges();
  return fixture;
}

describe('PostJob', () => {
  it('shows the create title without an id', () => {
    const fixture = setup({});
    expect(fixture.nativeElement.querySelector('h1').textContent).toContain('Post a new job');
  });

  it('loads the job and shows the edit title with an id', () => {
    const fixture = setup({ id: '1' });
    expect(fixture.nativeElement.querySelector('h1').textContent).toContain('Edit job');
    expect(fixture.componentInstance.job?.title).toBe('Frontend Developer');
  });
});