using System.Collections.Generic;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Games;

namespace MeetAndPlay.Web.ViewModels
{
    public class AddPersonalViewModel
    {
        public string Description { get; set; }
        public HashSet<Game> Games { get; set; } = new ();
        public PlaceType PlaceType { get; set; }
        public GameLevel GameLevel { get; set; }

        public PeriodViewModel PeriodViewModel { get; set; } = new();
    }
}