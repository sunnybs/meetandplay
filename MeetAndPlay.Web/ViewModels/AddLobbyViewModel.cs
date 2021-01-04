using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Games;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Web.ViewModels
{
    public class AddLobbyViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        [Required]
        public DateTime PlannedGameDate { get; set; }
        
        [Required]
        [Range(1, 100000, ErrorMessage = "Введите численное значение.")]
        public int MaxPlayersCount { get; set; }
        
        [Range(1, 100000, ErrorMessage = "Введите численное значение.")]
        public int CurrentPlayersCount { get; set; }
        public PlaceType PlaceType { get; set; }
        public GameLevel GameLevel { get; set; }
        public HashSet<Game> Games { get; set; } = new ();
        public HashSet<User> CurrentPlayers { get; set; } = new ();
    }
}