using System;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Data.Models.Places
{
    public class PlaceGame
    {
        public Guid PlaceId { get; set; }
        public Place Place { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}