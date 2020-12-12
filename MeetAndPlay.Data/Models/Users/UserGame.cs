using System;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Data.Models.Users
{
    public class UserGame
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}