import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { Account } from '../services/account';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(Account);
  const toastr = inject(ToastrService);
  const router = inject(Router);
  const token = localStorage.getItem('token');

  if (token) {
    return true;
  } else {
    toastr.error('You are not logged in');
    router.navigate(['']);
    return false;
  }
};
