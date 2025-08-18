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
import { ToastrService } from 'ngx-toastr';
import { ThemeService } from '../services/darktoggle';
import { TranslateService } from '@ngx-translate/core';
import { Presence } from '../services/presence';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.html',
  styleUrl: './nav.css',
  standalone: false
})
export class Nav {
  authData: authModel = { username: '', password: '' };
  currUserId:number|null = 0;
  user$: Observable<userModel | null>; 
  isLoggedIn = false;
  subscriptions: Subscription[] = [];
  isHomePage = false;
  searchTerm = '';
  filteredUsers: allUsersModel[] = [];
  allUsers: allUsersModel[] = [];
  showSearchResults = false;
  isDarkMode = false;
  currentLang: string | null = 'en';

  constructor(private presenceServ: Presence, private themeService: ThemeService, private authServ: Account, private translate: TranslateService
, private renderer: Renderer2, private userServ: User, private router: Router, private uiF: UiFunctions,private toastr:  ToastrService ) {
    this.user$ = this.authServ.CurrentUser$;
    this.currentLang = this.translate.currentLang || this.translate.getDefaultLang();
    const savedLang = localStorage.getItem('lang') || 'en';
    this.translate.use(savedLang);
  }


  ngOnInit(): void {
    this.subscriptions.push(this.user$.subscribe(user => {
      this.isLoggedIn = !!user;
      this.handleScroll();
      this.updateBodyClass();

    }));
    this.subscriptions.push(this.themeService.isDarkTheme.subscribe(dark => {
      this.isDarkMode = dark;
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

  switchLanguage() {
    this.currentLang = this.currentLang === 'en' ? 'ar' : 'en';
    this.translate.use(this.currentLang);
    localStorage.setItem('lang', this.currentLang);
  }

  toggleTheme() {
    this.isDarkMode = !this.isDarkMode;
    this.themeService.setDarkTheme(this.isDarkMode);
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
    this.router.navigate(['/members', user.username]);
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
          this.toastr.success('Login successful!', 'Welcome');
          this.router.navigate(['/feed']);
        }
      }   
    });
  }


  logout() {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.authData = { username: '', password: '' };
    this.authServ.setCurrentUser(null);
    this.presenceServ.stopHubConnection();
    this.router.navigate(['/']);
  }


  get homeLink(): string {
    return this.isLoggedIn ? '/feed' : '/';
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
