using System.Collections.Generic;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Places
{
    public class Place : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public PlaceType PlaceType { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}