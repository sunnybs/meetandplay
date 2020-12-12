using System.Collections.Generic;
using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Places
{
    public class Country : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}