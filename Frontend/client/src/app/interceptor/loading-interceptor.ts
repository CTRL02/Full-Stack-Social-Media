import { HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { Busy } from '../services/busy';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private busyService: Busy) {}
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    this.busyService.busy();
    return next.handle(req).pipe(delay(1000), finalize(() => this.busyService.idle()));
  }
}
