using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Interfaces;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Data.Models.Places
{
    public class City : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}