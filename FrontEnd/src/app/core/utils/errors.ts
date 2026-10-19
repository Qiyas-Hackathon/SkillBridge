import { HttpErrorResponse } from '@angular/common/http';

/**
 * Turns an API error (RFC 7807 problem details, validation errors, or a network
 * failure) into one readable message for the UI.
 */
export function errorMessage(err: unknown, fallback = 'Something went wrong. Please try again.'): string {
  if (!(err instanceof HttpErrorResponse)) return fallback;

  if (err.status === 0) {
    return 'Cannot reach the server. Check that the API is running.';
  }

  const body = err.error;
  if (body && typeof body === 'object') {
    // Validation problem: { errors: { Field: ['message'] } }
    if (body.errors && typeof body.errors === 'object') {
      const messages = Object.values(body.errors as Record<string, string[]>).flat();
      if (messages.length) return messages.join(' ');
    }
    if (typeof body.detail === 'string' && body.detail) return body.detail;
    if (typeof body.title === 'string' && body.title) return body.title;
  }

  if (err.status === 401) return 'Please sign in again.';
  if (err.status === 403) return 'You do not have permission to do that.';
  if (err.status === 404) return 'Not found.';
  return fallback;
}
