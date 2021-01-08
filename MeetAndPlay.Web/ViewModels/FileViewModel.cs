using System;

namespace MeetAndPlay.Web.ViewModels
{
    public class FileViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Filename { get; set; }
        public string MimeType { get; set; }
        public string Hash { get; set; }
        public string FileLink { get; set; }
        public bool IsNewFile { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is FileViewModel compareObj))
                return false;
            
            return compareObj.Id == Id;
        }
    }
}