import { Component, ViewEncapsulation } from '@angular/core';
import { UiFunctions } from '../services/ui-functions';
import { MessageService } from '../services/message-service';
import { CreateMessageDto } from '../models/CreateMessageDto';
import { MessageDto } from '../models/MessageDto';
import { Account } from '../services/account';
import { userModel } from '../models/user';
import { Subscription } from 'rxjs';

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
  user: userModel | null = null;
  isChatOpen: boolean = true;
  private subscriptions: Subscription[] = [];

  constructor(private uiFun: UiFunctions, private messageService: MessageService, private accountServ: Account) { }

  ngOnInit() {
    this.loadChats(); const pendingUsername = this.uiFun.getLastUsername();
    if (pendingUsername) { setTimeout(() => this.ensureChat(pendingUsername), 300); } this.uiFun.openChat$.subscribe(username => { if (username) { this.ensureChat(username); } });
  } private ensureChat(username: string) {
    if (!this.chats.some(c => c.username === username)) {
      this.chats.push({ username, messages: [] });
    } const newChat = this.chats.find(c => c.username === username);
    if (newChat) { this.selectChat(newChat); }
  }
  ngOnDestroy() {
    this.subscriptions.forEach(s => s.unsubscribe());
    this.subscriptions = [];
    this.messageService.stopHubConnection();
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
    this.messageService.stopHubConnection();
    this.isChatOpen = false;
  }
 
  selectChat(chat: any) {
    this.selectedChat = chat;

    this.messageService.createHubConnection(
      this.accountServ.getCurrentUser()!,
      chat.username
    );

    this.subscriptions.forEach(s => s.unsubscribe());
    this.subscriptions = [];

    const sub = this.messageService.messageThread$.subscribe(messages => {
      if (this.selectedChat?.username === chat.username) {
        this.selectedChat.messages = messages.map(m => ({
          text: m.content,
          self: m.senderUsername === this.accountServ.getUsernameFromToken(),
          isRead: !!m.dateRead, 
          dateRead: m.dateRead
        }));

        this.messageService.markMessagesAsRead(chat.username);
      }
    });

    this.subscriptions.push(sub);
  }



  sendMessage() {
    if (!this.newMessage.trim() || !this.selectedChat) return;

    const dto: CreateMessageDto = {
      content: this.newMessage,
      recipientUsername: this.selectedChat.username
    };

    this.messageService.sendMessage(dto).then(() => this.newMessage = '');
  }
}
