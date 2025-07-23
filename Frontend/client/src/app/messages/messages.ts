import { Component, ViewEncapsulation } from '@angular/core';
import { UiFunctions } from '../services/ui-functions';

@Component({
  selector: 'app-messages',
  standalone: false,
  templateUrl: './messages.html',
  styleUrl: './messages.css',
  encapsulation: ViewEncapsulation.None
})
export class Messages {
  isMinimized = false;
  selectedChat: any = null;
  newMessage: string = '';
  constructor(private uiFun:UiFunctions) { }
  dummyChats = [
    {
      username: 'Alice',
      messages: [
        { text: 'Hi there!', self: false },
        { text: 'Hey Alice!', self: true }
      ]
    },
    {
      username: 'Bob',
      messages: [
        { text: 'Wanna catch up later?', self: false },
        { text: 'Sure, what time?', self: true }
      ]
    }
  ];

  toggleMinimize() {
    this.isMinimized = !this.isMinimized;
  }

  closeChat() {
    this.isMinimized = false;
    this.selectedChat = null;
    this.uiFun.close();
  }
 
  selectChat(chat: any) {
    this.selectedChat = chat;
  }

  sendMessage() {
    if (!this.newMessage.trim() || !this.selectedChat) return;

    // Add message to current chat (you can later call your API here)
    this.selectedChat.messages.push({
      text: this.newMessage,
      self: true
    });

    // Clear input
    this.newMessage = '';

    // Optionally scroll down (later if needed)
  }
}
