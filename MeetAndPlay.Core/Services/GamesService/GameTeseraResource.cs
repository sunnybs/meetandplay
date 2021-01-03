using System;
using MeetAndPlay.Data.Models.Games;
using Newtonsoft.Json;

namespace MeetAndPlay.Core.Services.GamesService
{
    [JsonObject]
    public class GameTeseraResource
    {
        public int TeseraId { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string DescriptionShort { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public string PhotoUrl { get; set; }
        public int Year { get; set; }
        public int PlayersMin { get; set; }
        public int PlayersMax { get; set; }
        public int PlayersAgeMin { get; set; }
        public int PlaytimeMin { get; set; }
        public int PlaytimeMax { get; set; }

        public Game ToGame()
        {
            return new Game
            {
                Name = Title,
                DescriptionHtml = Description,
                Year = Year,
                CreationDate = CreationDateUtc,
                AgeRestriction = PlayersAgeMin,
                AveragePlaytimeInMinutes = (PlaytimeMin + PlaytimeMax) / 2,
                MinPlayersCount = PlayersMin,
                MaxPlayersCount = PlayersMax,
                IsActive = true
            };
        }
    }
}