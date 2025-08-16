import { Injectable } from '@angular/core';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class Busy {

  busyRequestCount = 0;
  constructor(private spinnerServ: NgxSpinnerService) { }

  busy() {
    this.busyRequestCount++;


    this.spinnerServ.show(undefined, { type: 'pacman' });

  }


  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerServ.hide();
    }
  }


}
