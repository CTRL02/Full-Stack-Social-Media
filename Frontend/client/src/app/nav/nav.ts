import { Component, HostListener, Renderer2 } from '@angular/core';
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
  errorMsg = '';

  user$: Observable<userModel | null>; 

  constructor(private authServ: Account, private renderer: Renderer2) {
    this.user$ = this.authServ.CurrentUser$;  
  }


  ngOnInit(): void {
    // Ensure initial scroll state is checked
    this.handleScroll();
  }    

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    this.handleScroll();
  }

  handleScroll(): void {
    const nav = document.querySelector('.scrolling-navbar');
    if (nav) {
      if (window.scrollY > 100) {
        this.renderer.addClass(nav, 'scrolled');
      } else {
        this.renderer.removeClass(nav, 'scrolled');
      }
    }
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



 
}
