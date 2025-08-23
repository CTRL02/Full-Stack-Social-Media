using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using socialmedia.DTOs;
using socialmedia.Entities;
using socialmedia.Repositories.MessageService;
using System.Security.Claims;

namespace socialmedia.SignalR
{
    [Authorize]
    public class MessageHub:Hub
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly PresenceTracker _tracker;
        private readonly IHubContext<PresenceHub> _hubContext;
        public MessageHub(IMessageService messageService, IMapper mapper, IUserService userService, PresenceTracker tracker, IHubContext<PresenceHub> hubContext)
        {
            _mapper = mapper;
            _messageService = messageService;
            _userService = userService;
            _tracker = tracker;
            _hubContext = hubContext;
        }
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(username, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(Context,groupName);
            var group = await _messageService.GetMessagesThread(username, otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", group);

        }

        public string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveFromGroup(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task sendMessage(CreateMessageDto createMessageDto)
        {
            var userName = Context.User.FindFirst(ClaimTypes.Name)?.Value!;
            if (userName == createMessageDto.RecipientUsername)
            {
                throw new HubException("You cannot send a message to yourself.");
            }
            var sender = await _userService.GetUserByNameDuplicate(userName);
            var recipient = await _userService.GetUserByNameDuplicate(createMessageDto.RecipientUsername);
            if (sender == null || recipient == null)
            {
                throw new HubException("User not found.");
            }

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content

            };
            var groupName = GetGroupName(userName, createMessageDto.RecipientUsername);
            var group = await _messageService.GetMessageGroup(groupName);
            if(group.connections.Any(x => x.Username == recipient.UserName))
            {
                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
            }
            else
            {
                message.IsRead = false;
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections!= null)
                {
                    await _hubContext.Clients.Clients(connections)
                        .SendAsync("NewMessageReceived", sender.UserName);
                }


            }
            _messageService.AddMessage(message);
            if (await _messageService.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
                return;
            }

            throw new HubException("Failed to send message.");

        }
        public async Task<bool> AddToGroup(HubCallerContext Context,string groupName)
        {
            var userName = Context.User.FindFirst(ClaimTypes.Name)?.Value!;
            var group = await _messageService.GetMessageGroup(groupName);
            var connection = new Connection(userName, Context.ConnectionId);
            if(group == null)
            {
                group = new Group(groupName);
                _messageService.addGroup(group);
            }
            group.connections.Add(connection);
            return await _messageService.SaveAllAsync();
        }

        private async Task RemoveFromGroup(string connectionId)
        {
            var connect = await _messageService.GetConnection(connectionId);
            _messageService.removeConnection(connect);
            await _messageService.SaveAllAsync();
        }

        public async Task MarkMessagesAsRead(string otherUsername)
        {
            var currentUsername = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            var messages = await _messageService.GetUnreadMessages(currentUsername, otherUsername);

            if (messages.Any())
            {
                foreach (var message in messages)
                {
                    message.IsRead = true;
                    message.ReadAt = DateTime.UtcNow;
                }

                await _messageService.SaveAllAsync();

                await Clients.User(otherUsername).SendAsync("MessagesRead", currentUsername);
            }
        }

    }
}
