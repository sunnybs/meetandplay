using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Chat
{
    public class Chat : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public bool IsPersonalChat { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ChatUser> ChatUsers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}