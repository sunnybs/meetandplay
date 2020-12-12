using System;

namespace MeetAndPlay.Data.Models.Places
{
    public class Location : BaseEntity
    {
        public string Address { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Guid PlaceId { get; set; }
        public Place Place { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public int StartWorkHour { get; set; }
        public int EndWorkHour { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }
    }
}