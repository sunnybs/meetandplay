using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MeetAndPlay.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        public const string HubUrl = "/MessagesHub";
        
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        
        public async Task UpdateChatAsync(Guid chatId)
        {
            await Clients.Group(chatId.ToString()).SendAsync("UpdateChat", chatId);
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            _logger.LogInformation($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }

        public async Task EnterChatAsync(Guid chatId, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        
        public async Task LeaveChatAsync(Guid chatId, string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}