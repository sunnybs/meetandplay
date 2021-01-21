using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Chat
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public Guid ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<MessageReceivers> Receivers { get; set; }
    }
}