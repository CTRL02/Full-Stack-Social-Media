import { Injectable } from '@angular/core';
import { allUsersModel } from '../models/allusersModel';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';
import { UserProfile } from '../models/userProfile';
 
@Injectable({
  providedIn: 'root'
})
export class User {
  baseUrl = 'http://localhost:5080/api/User/';
  constructor(private http: HttpClient) { }
  //populate active users on homepage
  getUsers(){
    return this.http.get<allUsersModel[]>(this.baseUrl + 'getusers');
  }


  getUserIdFromToken(token: string): number | null {

    const decoded: any = jwtDecode(token);
    return decoded?.nameid || decoded?.sub || decoded?.userId || null;
 
  }

  getUserBySearch(searchTerm: string) {
    return this.http.get<allUsersModel[]>(this.baseUrl + 'getuserswithterm/' + searchTerm);
  }

  //populate profile page with this endpoint
  //making this endpoint return id would scale the app as it will remove the need to call other apis that gets the user id ..will be added later...
  getUserByUsername(username: string): Observable<UserProfile> {
    return this.http.get<UserProfile>(this.baseUrl + 'name/' + username);
  }

}
