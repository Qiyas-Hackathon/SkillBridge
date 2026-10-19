import { RenderMode, ServerRoute } from '@angular/ssr';

// Only the public pages are prerendered. Everything behind a login depends on the
// browser's stored token, so it renders on the client.
export const serverRoutes: ServerRoute[] = [
  { path: '', renderMode: RenderMode.Prerender },
  { path: 'login', renderMode: RenderMode.Prerender },
  { path: 'register', renderMode: RenderMode.Prerender },
  { path: '**', renderMode: RenderMode.Client },
];
