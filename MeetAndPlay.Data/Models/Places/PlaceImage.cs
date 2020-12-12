using System;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Places
{
    public class PlaceImage
    {
        public Guid PlaceId { get; set; }
        public Place Place { get; set; }
        public Guid FileId { get; set; }
        public File File { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}