using System;

namespace MeetAndPlay.Offers.Data.DTO
{
    public class ShortNewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Poster { get; set; }
        public DateTime Date { get; set; }
    }
}