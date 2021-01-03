using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Games
{
    //TODO: Подумать насчёт рейтинга
    public class Game : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public virtual ICollection<GameGenre> GameGenres { get; set; }
        public virtual ICollection<GameImage> GameImages { get; set; }
        public string Description { get; set; }
        public string DescriptionHtml { get; set; }
        public int Year { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }
        public int AgeRestriction { get; set; }
        public int AveragePlaytimeInMinutes { get; set; }
    }
}