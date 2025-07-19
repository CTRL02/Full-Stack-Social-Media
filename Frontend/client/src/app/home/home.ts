import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { registerUser } from '../models/registerUser';
import { allUsersModel } from '../models/allusersModel';
import { Observable } from 'rxjs';
import { User } from '../services/user';
import { Account } from '../services/account';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  model: registerUser = ({ username: '', avatar: '', password: '' });
  errorMsg: string = '';

  constructor(private userService: User, private accountService: Account) { }

  contentItems = [
    {
      title: 'Friend Suggestions',
      body: 'Find and connect with new people based on your interests.',
      bgImage: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=1400&q=80'
    },
    {
      title: 'New Messages',
      body: 'Check out your unread messages and respond quickly.',
      bgImage: 'https://www.n2growth.com/wp-content/uploads/2020/06/messaging-by-desk.jpg'
    }
  ];
  sectionInView: boolean[] = [];

  activeUsers$!: Observable<allUsersModel[]>;
  @ViewChild('registerForm') registerForm!: NgForm;
  @ViewChild('registerFormContainer') registerFormContainer!: ElementRef;

  showRegisterForm = false;

  toggleRegisterForm() {

    this.showRegisterForm = true;
    setTimeout(() => {
      this.registerFormContainer?.nativeElement?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 100);
  }



  ngOnInit() {
    this.sectionInView = new Array(this.contentItems.length).fill(false);
    this.onWindowScroll();
    this.activeUsers$ = this.userService.getUsers();

  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const navbar = document.getElementById('mainNavbar');
    if (navbar) {
      navbar.classList.toggle('scrolled', window.scrollY > 180);
    }

    // Fade in content boxes
    document.querySelectorAll('.content-box').forEach((box: Element) => {
      const rect = box.getBoundingClientRect();
      if (rect.top < window.innerHeight - 100) {
        box.classList.add('visible');
      }
    });

    // Fade in dynamic sections
    const sections = document.querySelectorAll('.dynamic-section');
    sections.forEach((section, index) => {
      const rect = section.getBoundingClientRect();
      if (rect.top < window.innerHeight - 100 && !this.sectionInView[index]) {
        this.sectionInView[index] = true;
      }
    });
  }


  register() {
    this.model.avatar=this.userService.getRandomAvatar();
    this.accountService.register(this.model).subscribe({
      next: user => {
        if (user) {
          console.log('registered as user and logged in:', user);
        }
        this.errorMsg = '';
      },
      error: err => {
        if (err.status === 400) {
          this.errorMsg = 'Please enter valid data.';
        } else if (err.status === 401) {
          this.errorMsg = 'Invalid username or password.';
        } else {
          this.errorMsg = 'An unexpected error occurred.';
        }
      }
    });
    this.registerForm.resetForm();

  }




}


