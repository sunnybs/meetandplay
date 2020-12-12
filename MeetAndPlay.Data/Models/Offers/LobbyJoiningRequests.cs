using System;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Games;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Offers
{
    public class LobbyJoiningRequests
    {
        public Guid LobbyId { get; set; }
        public virtual Lobby Lobby { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string InitialMessage { get; set; }
        public DateTime InitialDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public RequestInitiator RequestInitiator { get; set; }
    }
}