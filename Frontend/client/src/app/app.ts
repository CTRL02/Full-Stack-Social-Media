import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { userModel } from './models/user';
import { Account } from './services/account';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {
  constructor(private HttpClient: HttpClient, private accountServ:Account) { }

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
