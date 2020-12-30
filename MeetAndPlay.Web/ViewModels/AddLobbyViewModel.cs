using System;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Web.ViewModels
{
    public class AddLobbyViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PlannedGameDate { get; set; }
        public int MaxPlayersCount { get; set; }
        public int CurrentPlayersCount { get; set; }
        public PlaceType PlaceType { get; set; }
        public NamedViewModel[] Games { get; set; }
        public NamedViewModel[] CurrentPlayers { get; set; }
        public ImageViewModel[] Images { get; set; }
    }
}