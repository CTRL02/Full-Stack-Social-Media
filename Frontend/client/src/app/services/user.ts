import { Injectable } from '@angular/core';
import { allUsersModel } from '../models/allusersModel';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class User {
  baseUrl = 'http://localhost:5080/api/User/';
  constructor(private http: HttpClient) { }

  getUsers(){
    return this.http.get<allUsersModel[]>(this.baseUrl + 'getusers');
  }

  getRandomAvatar(): string {
    const randomId = Math.floor(Math.random() * 70) + 1;
    return `https://i.pravatar.cc/150?img=${randomId}`;
  }

}
