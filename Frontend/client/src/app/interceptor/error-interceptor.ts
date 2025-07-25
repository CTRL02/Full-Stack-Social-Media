import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 400:
            this.toastr.error(error.error?.message || 'Invalid input', 'Bad Request');
            break;
          case 401:
            this.toastr.warning('Please login again', 'Unauthorized');
            this.router.navigate(['/']);
            break;
          case 403:
            this.toastr.error('Access denied', 'Forbidden');
            break;
          case 404:
            this.router.navigate(['/not-found']);
            break;
          case 500:
            this.router.navigate(['/server-error']);
            break;
          default:
            this.toastr.error('An unexpected error occurred', 'Error');
            break;
        }

        return throwError(() => error);
      })
    );
  }
}
