using System;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Places
{
    public class PlaceImage : FileWithCompressedCopy
    {
        public Guid PlaceId { get; set; }
        public Place Place { get; set; }
        public bool IsCurrentPoster { get; set; }
    }
}