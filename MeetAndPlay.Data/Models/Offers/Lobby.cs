using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.Models.Offers
{
    /// <summary>
    ///     Коммандное лобби для организации игры. Отображаются в поиске комманд.
    /// </summary>
    public class Lobby : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime PlannedGameDate { get; set; }
        public int MaxPlayersCount { get; set; }
        public int CurrentPlayersCount { get; set; }
        public bool IsActive { get; set; }
        public PlaceType PlaceType { get; set; }
        
        public ICollection<LobbyGame> LobbyGames { get; set; }
        public ICollection<LobbyImage> LobbyImages { get; set; }
        public ICollection<LobbyPlayer> LobbyPlayers { get; set; }
        public ICollection<LobbyJoiningRequest> LobbyJoiningRequests { get; set; }
    }
}