using System;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Data.DTO.Lobies
{
    public class MobileLobbyAdd
    {
        public string Title { get; set; }
        public string NameOfTheGame { get; set; }
        public GameLevel GameLevel { get; set; }
        public string Description { get; set; }
        public int CurrentPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime PlannedGameDate { get; set; }
        public PlaceType PlaceType { get; set; }
        public string UserName { get; set; }
    }
}