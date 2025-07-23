import { Component } from '@angular/core';
import { allUsersModel } from '../models/allusersModel';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../services/user';

@Component({
  selector: 'app-memeber-profile',
  standalone: false,
  templateUrl: './memeber-profile.html',
  styleUrl: './memeber-profile.css'
})
export class MemeberProfile {
  user: allUsersModel = { username: '', avatar: '' };

  constructor(private router: Router, private userService: User, private route: ActivatedRoute) { }

  ngOnInit(): void {
    const passedUser = history.state?.user;
    console.log(passedUser);
    if (passedUser && typeof passedUser === 'object') {
      this.user = passedUser;
    } else {
      const username = this.route.snapshot.paramMap.get('username');
      if (username) {
        this.userService.getUserByUsername(username).subscribe(user => {
          this.user = user;
        });
      }
    }
  }

  userD = {
    username: 'JaneDoe',
    avatar: 'https://i.pravatar.cc/150?img=1',
    title: 'Premium Member',
    bio: 'Digital creator passionate about web development and UI/UX design.'
  };
}
