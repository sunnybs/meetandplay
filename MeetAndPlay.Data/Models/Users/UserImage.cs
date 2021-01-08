using System;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Users
{
    public class UserImage : FileWithCompressedCopy
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsCurrentAvatar { get; set; }
    }
}