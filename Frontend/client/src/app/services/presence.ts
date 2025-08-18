import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { userModel } from '../models/user';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class Presence {
  hubUrl = 'http://localhost:5080/hubs/presence';
  private hubConnection: HubConnection | null = null;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();
  constructor(private toastr: ToastrService) { }


  createHubConnection(user: userModel) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()
    this.hubConnection.start().catch(error => console.log(error));
    this.hubConnection.on('UserIsOnline', username => { this.toastr.info(username + ' has connected'); });
    this.hubConnection.on('UserIsOffline', username => { this.toastr.warning(username + ' has disconnected'); });

    this.hubConnection.on('GetOnlineUsers', (users: string[]) => {
      this.onlineUsersSource.next(users);
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
