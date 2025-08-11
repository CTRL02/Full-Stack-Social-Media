import { Component, ElementRef, HostListener, ViewChild, ViewEncapsulation } from '@angular/core';
import { registerUser } from '../models/registerUser';
import { allUsersModel } from '../models/allusersModel';
import { Observable } from 'rxjs';
import { User } from '../services/user';
import { Account } from '../services/account';
import { NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css',
  encapsulation: ViewEncapsulation.None
})
export class Home {
  model: registerUser = ({ username: '', avatar: null, password: '' , title: '', bio: '' });
  errorMsg: string = '';
  selectedFile: File| null = null;
  avatarPreviewUrl: string | ArrayBuffer | null = null;



  constructor(private userService: User, private accountService: Account, private toastr: ToastrService,private router: Router) { }

  contentItems = [
    {
      titleKey: 'HOME.FRIEND_SUGGESTIONS_TITLE',
      bodyKey: 'HOME.FRIEND_SUGGESTIONS_BODY',
      bgImage: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=1400&q=80'
    },
    {
      titleKey: 'HOME.NEW_MESSAGES_TITLE',
      bodyKey: 'HOME.NEW_MESSAGES_BODY',
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

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;

      // Preview
      const reader = new FileReader();
      reader.onload = () => {
        this.avatarPreviewUrl = reader.result; // This will update the <img> tag
      };
      reader.readAsDataURL(file);

      this.model.avatar = file;
    }
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
    const formData = new FormData();

    formData.append('Username', this.model.username);
    formData.append('Password', this.model.password);
    formData.append('title', this.model.title);
    formData.append('bio', this.model.bio);

    if (this.selectedFile) {
      formData.append('Photo', this.selectedFile);
    }

    this.accountService.register(formData).subscribe({
      next: user => {
        if (user) {
          this.toastr.success('Registration successful as ' + user.username);
          this.avatarPreviewUrl = null;
          this.router.navigate(['feed']);
        }
        this.errorMsg = '';
        this.registerForm.resetForm();
      },
      error: err => {
        if (err.status === 400) {
          this.toastr.error('Please enter valid data.');
        } else if (err.status === 401) {
          this.toastr.error('Invalid username or password.');
        } else {
          this.toastr.error("An unexpected error occurred.");
        }
      }
    });
  }




}


