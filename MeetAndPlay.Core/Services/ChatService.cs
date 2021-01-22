using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Data.DTO.Chat;
using MeetAndPlay.Data.Models.Chat;
using MeetAndPlay.Data.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class ChatService : IChatService
    {
        private readonly DbContextFactory _contextFactory;
        private readonly IUserService _userService;

        public ChatService(DbContextFactory contextFactory, IUserService userService)
        {
            _contextFactory = contextFactory;
            _userService = userService;
        }

        public async Task<Guid> CreateChatWithUserAsync(Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var alreadyCreatedChat = await mnpContext.Chats
                .FirstOrDefaultAsync(c => c.IsPersonalChat
                                          && c.ChatUsers.Select(c => c.UserId).Contains(currentUserId)
                                          && c.ChatUsers.Select(c => c.UserId).Contains(userId));
            if (alreadyCreatedChat != null)
                return alreadyCreatedChat.Id;

            var chat = new Chat
            {
                Name = "Переписка",
                CreationDate = DateTime.Now,
                IsPersonalChat = true,
                ChatUsers = new List<ChatUser>
                {
                    new() {UserId = currentUserId, IsCreator = true}, new() {UserId = userId, IsCreator = false}
                }
            };

            await mnpContext.AddAsync(chat);
            await mnpContext.SaveChangesAsync();
            return chat.Id;
        }
        
        public async Task<Guid> CreateChatAsync(string title, bool isPersonalChat)
        {
            var chat = new Chat
            {
                Name = title, 
                CreationDate = DateTime.Now,
                IsPersonalChat = isPersonalChat
            };
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            chat.ChatUsers = new List<ChatUser>
            {
                new() {UserId = currentUserId, IsCreator = true}
            };

            await using var mnpContext = _contextFactory.Create();
            await mnpContext.AddAsync(chat);
            await mnpContext.SaveChangesAsync();
            return chat.Id;
        }

        public async Task<Guid> UpdateChatTitleAsync(Guid chatId, string title)
        {
            await using var mnpContext = _contextFactory.Create();
            var chat = await mnpContext.Chats.FindAsync(chatId);
            chat.Name = title;
            await mnpContext.SaveChangesAsync();
            return chat.Id;
        }

        public async Task AddUserToChatAsync(Guid chatId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            if (await mnpContext.ChatUsers.AnyAsync(u => u.UserId == userId && u.ChatId == chatId)) return;

            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                IsCreator = false
            };
            await mnpContext.AddAsync(chatUser);
            await mnpContext.SaveChangesAsync();
        }

        public async Task RemoveUserFromChatAsync(Guid chatId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            var chatUser =
                await mnpContext.ChatUsers.SingleOrDefaultAsync(u => u.UserId == userId && u.ChatId == chatId);
            if (chatUser == null) return;

            mnpContext.Remove(chatUser);
            await mnpContext.SaveChangesAsync();
        }

        public async Task<User[]> GetReceiversFromChatAsync(Guid chatId)
        {
            await using var mnpContext = _contextFactory.Create();
            var receivers = await mnpContext.ChatUsers
                .Include(c => c.User)
                .Where(c => c.ChatId == chatId)
                .AsNoTracking()
                .Select(c => c.User)
                .ToArrayAsync();

            return receivers;
        }

        public async Task<Guid> AddMessageToChatAsync(Guid chatId, string messageText)
        {
            await using var mnpContext = _contextFactory.Create();
            var message = new Message
            {
                ChatId = chatId,
                CreationDate = DateTime.Now.Date,
                Text = messageText
            };
            await mnpContext.AddAsync(message);
            var receivers = await GetReceiversFromChatAsync(chatId);
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var userMessages = receivers.Select(r => new MessageReceivers
            {
                MessageId = message.Id,
                UserId = r.Id,
                IsViewed = false,
                IsCreator = r.Id == currentUserId
            });
            await mnpContext.AddRangeAsync(userMessages);
            await mnpContext.SaveChangesAsync();
            return message.Id;
        }

        public async Task<MessageDto[]> GetMessagesAsync(Guid chatId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            return await mnpContext
                .MessageReceivers
                .Where(r => r.UserId == userId && r.Message.ChatId == chatId)
                .AsNoTracking()
                .Select(r => new MessageDto
                {
                    Id = r.MessageId,
                    Author = r.Message.Receivers.FirstOrDefault(u => u.IsCreator).User,
                    CreationDate = r.Message.CreationDate,
                    IsViewed = r.IsViewed,
                    Text = r.Message.Text
                }).ToArrayAsync();
        }

        public async Task SetMessageViewedAsync(Guid messageId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            var message =
                await mnpContext.MessageReceivers.SingleOrDefaultAsync(m =>
                    m.UserId == userId && m.MessageId == messageId);

            if (message == null)
                return;

            message.IsViewed = true;
            await mnpContext.SaveChangesAsync();
        }

        public async Task<Chat[]> GetUserChatsAsync(Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            return await mnpContext
                .Chats
                .Where(c => c.ChatUsers.Select(u => u.UserId).Contains(userId))
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<string> GetChatTitleForCurrentUserAsync(Guid chatId)
        {
            await using var mnpContext = _contextFactory.Create();
            var chat = await mnpContext.Chats
                .Include(c => c.ChatUsers)
                .ThenInclude(u => u.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                throw new NotFoundException();

            if (!chat.IsPersonalChat)
                return chat.Name;

            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var partnerUserName = chat.ChatUsers.FirstOrDefault(u => u.UserId != currentUserId)?.User.UserName;

            return partnerUserName;
        }
    }
}