using System;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Users
{
    public class UserImage
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
        public bool IsCurrentAvatar { get; set; }
    }
}