import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authModel } from '../models/authmodel';
import { map, ReplaySubject } from 'rxjs';
import { userModel  } from '../models/user';
import { registerUser } from '../models/registerUser';
import { Presence } from './presence';
@Injectable({
  providedIn: 'root'
})
export class Account {
  baseUrl = 'http://localhost:5080/api/Account/';
  constructor(private http: HttpClient, private presenceServ: Presence) { }
  private currentUser = new ReplaySubject<userModel|null>(1);
  CurrentUser$ = this.currentUser.asObservable();

 

  getUsernameFromToken(): string | null {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.unique_name || payload.sub || null;
    } catch (e) {
      console.error('Error decoding token', e);
      return null;
    }
  }

  login(logModel: authModel) {
    return this.http.post<userModel>(this.baseUrl + 'login', logModel).pipe(
      map((user: userModel) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user)); 
          localStorage.setItem('token', user.token);   
          this.currentUser.next(user);
          this.presenceServ.createHubConnection(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: userModel|null) {
    this.currentUser.next(user);
  }


  register(formData: FormData) {
    return this.http.post<userModel>(this.baseUrl + 'register', formData).pipe(
      map((user: userModel) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          localStorage.setItem('token', user.token);
          this.currentUser.next(user);
          this.presenceServ.createHubConnection(user);
        }
        return user;
      })
    );
  }
}
