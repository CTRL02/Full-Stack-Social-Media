import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { impressionToggleDto } from '../models/impressionToggleDto';

@Injectable({
  providedIn: 'root'
})
export class UserActions {
  private baseUrl = 'http://localhost:5080/api/Impressions';

  constructor(private http: HttpClient) { } 

  toggleImpression(dto: impressionToggleDto) {
    return this.http.post<{message: string}>(`${this.baseUrl}/leaveImpression`, dto);
  }
  
}
