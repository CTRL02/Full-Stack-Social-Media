import { Component, ViewEncapsulation } from '@angular/core';
import { UiFunctions } from '../services/ui-functions';
import { MessageService } from '../services/message-service';
import { CreateMessageDto } from '../models/CreateMessageDto';
import { MessageDto } from '../models/MessageDto';
import { Account } from '../services/account';

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
  chats: { username: string; messages: any[] }[] = [];
  constructor(private uiFun: UiFunctions, private messageService: MessageService, private accountServ: Account) { }
  ngOnInit() {
    this.loadChats();
    console.log("Chats grouped:", this.chats);
  }

  private loadChats() {
    // Load Inbox + Outbox, merge them
    this.messageService.getMessages("Inbox").subscribe({
      next: inbox => {
        this.messageService.getMessages("Outbox").subscribe({
          next: outbox => {
            const allMessages = [...inbox, ...outbox];

            // Group messages by the other participant
            const grouped = new Map<string, MessageDto[]>();
            allMessages.forEach(m => {
              const otherUser = m.senderUsername === this.accountServ.getUsernameFromToken()
                ? m.recipientUsername
                : m.senderUsername;

              if (!grouped.has(otherUser)) grouped.set(otherUser, []);
              grouped.get(otherUser)!.push(m);
            });

            this.chats = Array.from(grouped.entries()).map(([username, msgs]) => ({
              username,
              messages: msgs.map(m => ({
                text: m.content,
                self: m.senderUsername === this.accountServ.getUsernameFromToken()
              }))
            }));
          }
        });
      }
    });
  }

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
    this.messageService.getMessageThread(chat.username).subscribe({
      next: (messages) => {
        this.selectedChat.messages = messages.map(m => ({
          text: m.content,
          self: m.senderUsername !== chat.username
        }));
      }
    });
  }

  sendMessage() {
    if (!this.newMessage.trim() || !this.selectedChat) return;

    const dto: CreateMessageDto = {
      content: this.newMessage,
      recipientUsername: this.selectedChat.username
    };

    this.messageService.sendMessage(dto).subscribe({
      next: (sentMessage) => {
        this.selectedChat.messages.push({
          text: sentMessage.content,
          self: true
        });
        this.newMessage = '';
      }
    });
  }
}
