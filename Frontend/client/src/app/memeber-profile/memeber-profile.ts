import { Component } from '@angular/core';
import { allUsersModel } from '../models/allusersModel';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../services/user';
import { UserProfile } from '../models/userProfile';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-memeber-profile',
  standalone: false,
  templateUrl: './memeber-profile.html',
  styleUrl: './memeber-profile.css'
})
export class MemeberProfile {
  user: UserProfile | null = null;
  private destroy$ = new Subject<void>();

  constructor(private router: Router, private userService: User, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(takeUntil(this.destroy$)).subscribe(params => {
      const username = params.get('id');
      //console.log('Fetching user by username:', username);

      if (username) {
        this.userService.getUserByUsername(username).subscribe({
          next: user => {
            this.user = user;
          //  console.log('Fetched user from API:', user);
          },
          error: err => console.error('Error fetching user', err)
        });
      }
    });
  }


  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  getSocialIcon(link: string): string {
    if (link.includes('linkedin.com')) return 'fab fa-linkedin';
    if (link.includes('github.com')) return 'fab fa-github';
    if (link.includes('twitter.com')) return 'fab fa-twitter';
    if (link.includes('facebook.com')) return 'fab fa-facebook';
    return 'fas fa-globe'; 
  }

  
}
