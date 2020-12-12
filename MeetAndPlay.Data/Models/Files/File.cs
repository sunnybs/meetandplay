using System;

namespace MeetAndPlay.Data.Models.Files
{
    public class File : BaseEntity
    {
        public string Filename { get; set; }
        public string Hash { get; set; }
        public string MimeType { get; set; }
        public string FileLink { get; set; }
        public DateTime CreationDate { get; set; }
    }
}