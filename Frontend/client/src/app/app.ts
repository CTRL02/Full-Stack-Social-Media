import { Component, Renderer2 } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { userModel } from './models/user';
import { Account } from './services/account';
import { UiFunctions } from './services/ui-functions';
import { Observable } from 'rxjs';
import { ThemeService } from './services/darktoggle';
import { Presence } from './services/presence';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {

  showMessages$!: Observable<boolean>;
  currentTheme = 'light';


  constructor(private themeService: ThemeService, private HttpClient: HttpClient, private accountServ: Account, private uiFun: UiFunctions, private renderer: Renderer2, private presenceServ: Presence) {
    this.showMessages$ = this.uiFun.showMessages$;
  }


  ngOnInit() {
    this.setCurrentUser();
    this.themeService.isDarkTheme.subscribe(dark => {
      this.currentTheme = dark ? 'dark' : 'light';
    });

  }
  setCurrentUser() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user: userModel = JSON.parse(userJson);
      this.accountServ.setCurrentUser(user);
      this.presenceServ.createHubConnection(user);

    }
  }
}
