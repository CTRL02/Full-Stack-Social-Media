import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UiFunctions {
  private showMessagesSubject = new BehaviorSubject<boolean>(false);
  showMessages$ = this.showMessagesSubject.asObservable();

  private openChatSource = new BehaviorSubject<string | null>(null); // Use BehaviorSubject
  openChat$ = this.openChatSource.asObservable();

  private lastUsername: string | null = null;

  open() {
    this.showMessagesSubject.next(true);
  }

  close() {
    this.showMessagesSubject.next(false);
    this.openChatSource.next(null);
  }

  toggle() {
    this.showMessagesSubject.next(!this.showMessagesSubject.value);
  }

  openChatWith(username: string) {
    this.lastUsername = username;
    this.open();
    this.openChatSource.next(username);
  }

  getLastUsername(): string | null {
    return this.lastUsername;
  }
}
