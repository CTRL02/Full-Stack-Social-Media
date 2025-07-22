import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { userModel } from './models/user';
import { Account } from './services/account';
import { UiFunctions } from './services/ui-functions';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {


  showMessages$!: Observable<boolean>; 
  constructor(private HttpClient: HttpClient, private accountServ: Account, private uiFun: UiFunctions) {
    this.showMessages$ = this.uiFun.showMessages$;
  }


  ngOnInit() {
    this.setCurrentUser()

  }
  setCurrentUser() {
    const userJson = localStorage.getItem('user');
    if (userJson) {
      const user: userModel = JSON.parse(userJson);
      this.accountServ.setCurrentUser(user);
    }
  }
}
