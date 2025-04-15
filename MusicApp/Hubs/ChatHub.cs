using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MusicApp.Data;
using MusicApp.Models;

namespace MusicApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly MusicDbContext _context;

        public ChatHub(MusicDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string receiverId, string message, int? trackId = null)
        {
            var senderId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("User not authenticated");
            }

            var chatMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message1 = message,
                SentAt = DateTime.UtcNow, 
            };

            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.User(receiverId).SendAsync("ReceiveMessage", new
            {
                SenderId = senderId,
                Message = message,
                SentAt = chatMessage.SentAt,
                TrackId = trackId
            });
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendGroupMessage(string groupName, string message)
        {
            var senderId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(senderId))
            {
                throw new HubException("User not authenticated");
            }

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", new
            {
                SenderId = senderId,
                Message = message,
                SentAt = DateTime.UtcNow
            });
        }
    }
}