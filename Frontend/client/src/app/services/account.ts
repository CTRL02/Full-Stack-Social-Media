import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authModel } from '../models/authmodel';
import { map, ReplaySubject } from 'rxjs';
import { userModel  } from '../models/user';
import { registerUser } from '../models/registerUser';
@Injectable({
  providedIn: 'root'
})
export class Account {
  baseUrl = 'http://localhost:5080/api/Account/';
  constructor(private http: HttpClient) { }
  private currentUser = new ReplaySubject<userModel|null>(1);
  CurrentUser$ = this.currentUser.asObservable();

 

  
  login(logModel: authModel) {
    return this.http.post<userModel>(this.baseUrl + 'login', logModel).pipe(
      map((user: userModel) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user)); 
          localStorage.setItem('token', user.token);   
          this.currentUser.next(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: userModel|null) {
    this.currentUser.next(user);
  }


  register(regModel: registerUser) {
    return this.http.post<userModel>(this.baseUrl + 'register', regModel).pipe(
      map((user: userModel) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          localStorage.setItem('token', user.token);
          this.currentUser.next(user);
        }
        return user;
      })
    );
  }
}
