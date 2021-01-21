using System;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Chat
{
    public class ChatUser
    {
        public Guid ChatId { get; set; }
        public virtual Guid Chat { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsCreator { get; set; }
    }
}