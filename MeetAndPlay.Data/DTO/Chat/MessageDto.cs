using System;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.DTO.Chat
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsViewed { get; set; }
    }
}