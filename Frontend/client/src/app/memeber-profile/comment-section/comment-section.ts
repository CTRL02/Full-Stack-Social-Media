import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { ProfileCommentDto } from '../../models/ProfileCommentDto';

@Component({
  selector: 'app-comment-section',
  standalone: false,
  templateUrl: './comment-section.html',
  styleUrl: './comment-section.css'
})
export class CommentSection {
  @Input() postData!: ProfileCommentDto[];
  @Output() close = new EventEmitter<void>();

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const clickedInside = (event.target as HTMLElement).closest('.comment-modal');
    if (!clickedInside) {
      this.close.emit();
    }
  }

}
