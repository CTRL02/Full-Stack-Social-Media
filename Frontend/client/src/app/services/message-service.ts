import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, take } from 'rxjs';
import { CreateMessageDto } from '../models/CreateMessageDto';
import { MessageDto } from '../models/MessageDto';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { userModel } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private baseUrl = 'http://localhost:5080/api/Message';
  private hubUrl = 'http://localhost:5080/hubs/message'; 
  private hubConnection: HubConnection | null = null;
  private messageThreadSource = new BehaviorSubject<MessageDto[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  constructor(private http: HttpClient) { }

  createHubConnection(user: userModel, otherUsername: string) {
    if (this.hubConnection) {
      this.hubConnection.stop();
      this.hubConnection = null;
    }

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        const updatedMessages = [...messages, message];
        this.messageThreadSource.next(updatedMessages);
      });
    });

    this.hubConnection.on('MessagesRead', (username: string) => {
      console.log(`${username} read your messages`);

      this.messageThreadSource.pipe(take(1)).subscribe(messages => {
        const updated = messages.map(msg => {
          if (!msg.dateRead && msg.senderUsername === user.username) {
            return { ...msg, dateRead: new Date() };
          }
          return msg;
        });
        this.messageThreadSource.next(updated);
      });
    });


  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(error => console.log(error));
      this.hubConnection = null; 
    }
  }




  getMessages(container: 'Inbox' | 'Outbox' | 'Unread'): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(`${this.baseUrl}?container=${container}`);
  }

  getMessageThread(username: string): Observable<MessageDto[]> {
    return this.http.get<MessageDto[]>(`${this.baseUrl}/thread/${username}`);
  }

  async sendMessage(dto: CreateMessageDto) {
    return this.hubConnection?.invoke('SendMessage', dto).catch(error => console.log(error));
  }

  markMessagesAsRead(username: string) {
    return this.hubConnection?.invoke('MarkMessagesAsRead', username)
      .catch(err => console.log(err));
  }

}
