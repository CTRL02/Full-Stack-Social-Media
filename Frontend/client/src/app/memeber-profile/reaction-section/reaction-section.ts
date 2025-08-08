import { Component, EventEmitter, HostListener, Input, input, Output } from '@angular/core';
import { ImpressionComponentDto } from '../../models/impressionComponentDto';

@Component({
  selector: 'app-reaction-section',
  standalone: false,
  templateUrl: './reaction-section.html',
  styleUrl: './reaction-section.css'
})
export class ReactionSection {
  @Input() impressions: ImpressionComponentDto[] = [];
  @Output() close = new EventEmitter<void>();

  reactionTypes: string[] = ['Like', 'Love', 'Care', 'Sad', 'Angry'];
  reactionIcons: { [key: string]: string } = {
    Like: 'assets/reactions/like.png',
    Love: 'assets/reactions/love.png',
    Care: 'assets/reactions/care.png',
    Sad: 'assets/reactions/sad.png',
    Angry: 'assets/reactions/angry.png'
  };

  selectedType: string | null = null;

  get filteredImpressions(): ImpressionComponentDto[] {
    if (!this.selectedType) return this.impressions;
    return this.impressions.filter(i => i.type === this.selectedType);
  }

  selectReaction(type: string) {
    this.selectedType = type;
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.reaction-section-container')) {
      this.close.emit();
    }
  }



}
