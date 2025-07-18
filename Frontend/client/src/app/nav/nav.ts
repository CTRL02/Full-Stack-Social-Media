import { Component } from '@angular/core';
import { authModel } from '../models/authmodel';
import { Account } from '../services/account';
import { Observable } from 'rxjs';
import { userModel } from '../models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.html',
  styleUrl: './nav.css',
  standalone: false
})
export class Nav {
  authData: authModel = { username: '', password: '' };
  isRegisterMode = false;
  errorMsg = '';

  user$: Observable<userModel | null>; 

  constructor(private authServ: Account) {
    this.user$ = this.authServ.CurrentUser$;  
  }

  login() {
    this.authServ.login(this.authData).subscribe({
      next: user => {
        if (user) {
          console.log('Logged in user:', user);
        }
        this.errorMsg = '';
      },
      error: err => {
        if (err.status === 400) {
          this.errorMsg = 'Please enter valid data.';
        } else if (err.status === 401) {
          this.errorMsg = 'Invalid username or password.';
        } else {
          this.errorMsg = 'An unexpected error occurred.';
        }
      }
    });
  }

  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.authData = { username: '', password: '' };
    this.authServ.setCurrentUser(null);
  }

  register() {
    // Add logic
  }

  toggleMode(event: Event) {
    event.preventDefault();
    this.isRegisterMode = !this.isRegisterMode;
  }
}
