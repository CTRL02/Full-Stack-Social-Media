import { Component, HostListener, Renderer2 } from '@angular/core';
import { authModel } from '../models/authmodel';
import { Account } from '../services/account';
import { Observable, Subscription } from 'rxjs';
import { userModel } from '../models/user';
import { User } from '../services/user';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { UiFunctions } from '../services/ui-functions';
import { allUsersModel } from '../models/allusersModel';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.html',
  styleUrl: './nav.css',
  standalone: false
})
export class Nav {
  authData: authModel = { username: '', password: '' };
  errorMsg = '';
  currUserId:number|null = 0;
  user$: Observable<userModel | null>; 
  isLoggedIn = false;
  subscriptions: Subscription[] = [];
  isHomePage = false;
  searchTerm = '';
  filteredUsers: allUsersModel[] = [];
  allUsers: allUsersModel[] = [];
  showSearchResults = false;

  constructor(private authServ: Account, private renderer: Renderer2, private userServ: User, private router: Router, private uiF: UiFunctions) {
    this.user$ = this.authServ.CurrentUser$;  
  }


  ngOnInit(): void {
    this.subscriptions.push(this.user$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.handleScroll();
      this.updateBodyClass();
    }));

    this.subscriptions.push(this.userServ.getUsers().subscribe(users => {
      this.allUsers = users;
    }));

    this.subscriptions.push(this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.isHomePage = event.urlAfterRedirects === '/' || event.urlAfterRedirects === '/home' || event.urlAfterRedirects === '';
        this.handleScroll();
        this.updateBodyClass();
      }));

    this.handleScroll();
    this.updateBodyClass();
  }



  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());


  }
  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredUsers = [];
      return;
    }
    this.userServ.getUserBySearch(term).subscribe(users => this.filteredUsers = users);
  }

  goToUserProfile(user: allUsersModel): void {
    this.searchTerm = '';
    this.filteredUsers = [];
    this.showSearchResults = false;
    this.router.navigate(['/members', user.username], { state: { user } });
  }

  hideResultsWithDelay(): void {
    setTimeout(() => this.showSearchResults = false, 200);
  }

  openMessages() {
    this.uiF.open();
  }

  updateBodyClass() {
    const body = document.body;

    if (this.isHomePage) {
      this.renderer.removeClass(body, 'with-navbar');
      this.renderer.addClass(body, 'home-no-navbar');
    } else {
      this.renderer.removeClass(body, 'home-no-navbar');
      this.renderer.addClass(body, 'with-navbar');
    }
  }




  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    this.handleScroll();
  }

  handleScroll(): void {
    const nav = document.querySelector('.scrolling-navbar');
    if (!nav) return;

    if (this.isLoggedIn) {
      // Always solid navbar when logged in
      this.renderer.addClass(nav, 'scrolled');
    } else {
      // Scroll-based background when logged out
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
          this.router.navigate(['/']);
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



 getProfileLink() {
  const raw = localStorage.getItem('user');
  const user = raw ? JSON.parse(raw) : null;
  const name = user?.username;
  if (user && name) {
    this.router.navigate(['/members', name]);
  }
}



 
}
