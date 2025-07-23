import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

// theme.service.ts
@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private darkTheme = new BehaviorSubject<boolean>(
    localStorage.getItem('darkTheme') === 'true'
  );
  isDarkTheme = this.darkTheme.asObservable();

  setDarkTheme(isDark: boolean): void {
    localStorage.setItem('darkTheme', isDark.toString());
    this.darkTheme.next(isDark);
  }
}
