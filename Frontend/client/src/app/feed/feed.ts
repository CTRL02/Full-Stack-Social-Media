import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ProfilePostDto } from '../models/ProfilePostDto';
import { UserActions } from '../services/user-actions';
import { feedModel } from '../models/feedModel';
import { Account } from '../services/account';
import { identity } from 'rxjs';

interface Post {
  username: string;
  avatar: string;
  image: string;
  caption: string;
  timestamp: Date;
}

@Component({
  selector: 'app-feed',
  standalone: false,
  templateUrl: './feed.html',
  styleUrl: './feed.css',
  encapsulation: ViewEncapsulation.None
})
export class Feed {
  feedPosts: { post: ProfilePostDto; user: { username: string; avatar: string } }[] = [];

  constructor(private feedService: UserActions, private accountService: Account) { }
  ngOnInit(): void {
    const username = this.accountService.getUsernameFromToken();

    this.feedService.getRecommendedPosts(username, 4).subscribe({
      next: (apiPosts) => {
        this.feedPosts = apiPosts.map(p => {
          const user = {
            username: p.UserName || '',
            avatar: p.avatar || 'assets/default.jpg'
          };

          const post: ProfilePostDto = {
            id: p.Id,                       
            content: p.Content,          
            createdAt: new Date(p.CreatedAt).toISOString(),
            impressions: p.impressions ?? [],
            comments: p.comments ?? []
          };

          return { post, user };
        });
      },
      error: err => {
        console.error('Failed to fetch recommendations', err);
      }
    });
  }




}
