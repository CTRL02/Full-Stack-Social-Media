import { Component, Input } from '@angular/core';
import { ProfilePostDto } from '../../models/ProfilePostDto';
import { UserActions } from '../../services/user-actions';
import { Account } from '../../services/account';
import { ProfileCommentDto } from '../../models/ProfileCommentDto';

@Component({
  selector: 'app-post',
  standalone: false,
  templateUrl: './post.html',
  styleUrl: './post.css'
})
export class Post {


  constructor(private userAction: UserActions,private authService: Account) { }


  @Input() post!: ProfilePostDto;
  @Input() user: any;


  selectedReactions: { [postId: number]: string } = {};
  openPostId: number | null = null;
  isExpanded: boolean = false;
  showReactions: boolean = false;


  readonly maxLength = 300;

  reactionTypes: string[] = ['Like', 'Love', 'Care', 'Sad', 'Angry'];

  reactionIcons: { [key: string]: string } = {
    Like: 'assets/reactions/like.png',
    Love: 'assets/reactions/love.png',
    Care: 'assets/reactions/care.png',
    Sad: 'assets/reactions/sad.png',
    Angry: 'assets/reactions/angry.png'
  };

  ngOnInit() {
    const currentUsername = this.authService.getUsernameFromToken();
    if (this.post?.impressions && currentUsername) {
      const userImpression = this.post.impressions.find(
        imp => imp.username === currentUsername
      );
      if (userImpression) {
        this.selectedReactions[this.post.id] = userImpression.type;
      }
    }
  }


  get impressionCount(): number {
    return this.post?.impressions?.length || 0;
  }

  get commentCount(): number {
    return this.post?.comments?.length || 0;
  }

  get trimmedContent(): string {
    return this.post?.content?.length > this.maxLength && !this.isExpanded
      ? this.post.content.substring(0, this.maxLength) + '...'
      : this.post.content;
  }

  toggleReadMore(): void {
    this.isExpanded = !this.isExpanded;
  }

  leaveImpression(type: string, postId: number) {
    const currentUsername = this.authService.getUsernameFromToken();
    if (!currentUsername) return;

    const previousReaction = this.selectedReactions[postId];
    const wasSameReaction = previousReaction === type;

    // Optimistic update
    const updatedImpressions = wasSameReaction
      ? this.post.impressions.filter(imp => imp.username !== currentUsername)
      : [
        ...this.post.impressions.filter(imp => imp.username !== currentUsername),
        {
          type,
          username: currentUsername,
          avatar:  'assets/default.jpg',
          createdAt: new Date().toISOString()
        }
      ];

    this.post = { ...this.post, impressions: updatedImpressions };

    if (wasSameReaction) {
      delete this.selectedReactions[postId];
    } else {
      this.selectedReactions[postId] = type;
    }

    this.userAction.toggleImpression({ type, postId }).subscribe({
      error: (err) => {
        // Revert on error
        this.post = {
          ...this.post,
          impressions: previousReaction
            ? [
              ...this.post.impressions.filter(imp => imp.username !== currentUsername),
              {
                type: previousReaction,
                username: currentUsername,
                avatar: this.user?.avatar || 'assets/default.jpg',
                createdAt: new Date().toISOString()
              }
            ]
            : this.post.impressions.filter(imp => imp.username !== currentUsername)
        };
        this.selectedReactions[postId] = previousReaction;
      }
    });
  }


  openComments(postId: number): void {
    this.openPostId = postId;
  }

  closeComments(): void {
    this.openPostId = null;
  }

  onCommentAdded(event: { postId: number; comment: ProfileCommentDto; parentCommentId?: number }) {
    if (this.post.id !== event.postId) return;

    if (event.parentCommentId) {
      const parent = this.post.comments.find(c => c.id === event.parentCommentId);
      if (parent) parent.replies.push(event.comment);
    } else {
      this.post.comments.push(event.comment);
    }
  }

  //related to reaction component
  openReactionPostId: number | null = null;

  openReactions(postId: number) {
    this.openReactionPostId = postId;
  }

  closeReactions() {
    this.openReactionPostId = null;
  }

  getMappedImpressions(postId: number) {
    if (this.post.id !== postId || !this.post.impressions) return [];
    return this.post.impressions.map(i => ({
      type: i.type,
      username: i.username,
      avatar: i.avatar
    }));
  }


}
