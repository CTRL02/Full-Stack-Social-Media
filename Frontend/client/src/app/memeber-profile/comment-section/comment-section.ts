import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { ProfileCommentDto } from '../../models/ProfileCommentDto';
import { UserActions } from '../../services/user-actions';
import { Account } from '../../services/account';

@Component({
  selector: 'app-comment-section',
  standalone: false,
  templateUrl: './comment-section.html',
  styleUrl: './comment-section.css'
})
export class CommentSection {
  @Input() postData!: ProfileCommentDto[];
  @Input() postId!: number;
  @Output() close = new EventEmitter<void>();
  @Output() commentAdded = new EventEmitter<{
    postId: number;
    comment: ProfileCommentDto;
    parentCommentId?: number;
  }>();
  constructor(private commentService: UserActions,private authService: Account) { }
  newCommentContent = '';
  replyingTo: number | null = null;
  replyContent = '';

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const clickedInside = (event.target as HTMLElement).closest('.comment-modal');
    if (!clickedInside) {
      this.close.emit();
    }
  }
  submitComment() {
    if (!this.newCommentContent.trim()) return;

    const content = this.newCommentContent.trim();
    const username = this.authService.getUsernameFromToken() || '';
    const avatar =  'assets/default.jpg';

    this.commentService.postComment(this.postId, content).subscribe({
      next: () => {
        const newComment: ProfileCommentDto = {
          id: Date.now(),
          content,
          createdAt: new Date().toISOString(),
          username,
          avatar,
          replies: [],
          impressions: []
        };

        this.newCommentContent = '';
        this.commentAdded.emit({
          postId: this.postId,
          comment: newComment
        });
      },
      error: (err) => console.error('Error posting comment', err)
    });
  }

  submitReply(commentId: number) {
    if (!this.replyContent.trim()) return;

    const content = this.replyContent.trim();
    const username = this.authService.getUsernameFromToken() || '' ;
    const avatar = 'assets/default.jpg';

    this.commentService.postComment(this.postId, content, commentId).subscribe({
      next: () => {
        const newReply: ProfileCommentDto = {
          id: Date.now(),
          content,
          createdAt: new Date().toISOString(),
          username,
          avatar,
          replies: [],
          impressions: []
        };

        this.replyContent = '';
        this.replyingTo = null;
        this.commentAdded.emit({
          postId: this.postId,
          comment: newReply,
          parentCommentId: commentId
        });
      },
      error: (err) => console.error('Error posting reply', err)
    });
  }


}
