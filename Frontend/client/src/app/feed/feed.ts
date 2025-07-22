import { Component, OnInit } from '@angular/core';

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
  styleUrl: './feed.css'
})
export class Feed {
  posts: Post[] = [];

  ngOnInit(): void {
    this.generateFakePosts(10);
  }

  generateFakePosts(count: number): void {
    const usernames = ['Alice', 'Bob', 'Charlie', 'Diana', 'Eve'];
    const captions = [
      'What a day!',
      'Feeling good 😊',
      'Just finished this!',
      'Loving this view!',
      'Random thought 💭'
    ];

    for (let i = 0; i < count; i++) {
      this.posts.push({
        username: usernames[Math.floor(Math.random() * usernames.length)],
        avatar: `https://i.pravatar.cc/150?img=${Math.floor(Math.random() * 70) + 1}`,
        image: `https://picsum.photos/seed/${Math.floor(Math.random() * 1000)}/600/400`,
        caption: captions[Math.floor(Math.random() * captions.length)],
        timestamp: new Date(Date.now() - Math.floor(Math.random() * 86400000)) // last 24 hrs
      });
    }
  }
}
