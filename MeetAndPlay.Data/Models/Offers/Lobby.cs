using System;
using System.Collections.Generic;

namespace MeetAndPlay.Data.Models.Offers
{
    /// <summary>
    ///     Коммандное лобби для организации игры. Отображаются в поиске комманд.
    /// </summary>
    public class Lobby
    {
        public Guid LobbyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime PlannedGameDate { get; set; }
        public int MaxPlayersCount { get; set; }
        public int CurrentPlayersCount { get; set; }
        public bool IsActive { get; set; }
        public Guid PlaceTypeId { get; set; }
        public ICollection<LobbyGame> LobbyGames { get; set; }
        public ICollection<LobbyImage> LobbyImages { get; set; }
    }
}