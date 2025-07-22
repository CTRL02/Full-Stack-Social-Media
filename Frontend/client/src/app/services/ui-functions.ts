import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UiFunctions {

  private showMessagesSubject = new BehaviorSubject<boolean>(false);
  showMessages$ = this.showMessagesSubject.asObservable();

  open() {
    this.showMessagesSubject.next(true);
  }

  close() {
    this.showMessagesSubject.next(false);
  }

  toggle() {
    this.showMessagesSubject.next(!this.showMessagesSubject.value);
  }

}
