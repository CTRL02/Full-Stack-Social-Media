import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateMessageDto } from '../models/CreateMessageDto';
import { MessageDto } from '../models/MessageDto';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private baseUrl = 'http://localhost:5080/api/Message'; // adjust to your backend

  constructor(private http: HttpClient) { }

  getMessages(container: 'Inbox' | 'Outbox' | 'Unread'): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(`${this.baseUrl}?container=${container}`);
  }

  getMessageThread(username: string): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(`${this.baseUrl}/thread/${username}`);
  }

  sendMessage(dto: CreateMessageDto): Observable<MessageDto> {
    return this.http.post<MessageDto>(`${this.baseUrl}/sendmessage`, dto);
  }
}
