using System;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Chat
{
    public class MessageReceivers
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
        public bool IsCreator { get; set; }
        public bool IsViewed { get; set; }
    }
}