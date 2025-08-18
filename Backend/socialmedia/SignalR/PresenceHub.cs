using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace socialmedia.SignalR
{
    [Authorize]
    //this hub is bugged as in case user opens multiplie tabs the user disconnected from one will be offline even though he is active on another
    //fixed soon
    //used presence tracker to track online users
    public class PresenceHub:Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            await _tracker.UserConnected(username, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", username);
            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers); 

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            await _tracker.UserDisconnected(username, Context.ConnectionId);
            var currentUsers = await _tracker.GetOnlineUsers();
            if (!currentUsers.Contains(username))
            {
                await Clients.Others.SendAsync("UserIsOffline", username);
            }
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
            await base.OnDisconnectedAsync(exception);

        }
    }
}
