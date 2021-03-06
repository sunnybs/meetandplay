using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.DTO.Chat;
using MeetAndPlay.Data.Models.Chat;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface IChatService
    {
        Task<Guid> CreateChatAsync(string title, bool isPersonal);
        Task<Guid> UpdateChatTitleAsync(Guid chatId, string title);
        Task<Guid> CreateChatWithUserAsync(Guid userId);
        Task AddUserToChatAsync(Guid chatId, Guid userId);
        Task RemoveUserFromChatAsync(Guid chatId, Guid userId);
        Task<User[]> GetReceiversFromChatAsync(Guid chatId);
        Task<Guid> AddMessageToChatAsync(Guid chatId, string messageText);
        Task<MessageDto[]> GetMessagesAsync(Guid chatId, Guid userId);
        Task SetMessageViewedAsync(Guid messageId, Guid userId);
        Task SetMessagesViewedAsync(Guid chatId, Guid userId);
        Task<Chat[]> GetUserChatsAsync(Guid userId);
        Task<int> GetNotViewedMessagesCountAsync(Guid userId);
        Task<int> GetNotViewedMessagesCountAsync(Guid userId, Guid chatId);
        Task<string> GetChatTitleForCurrentUserAsync(Guid chatId);
    }
}