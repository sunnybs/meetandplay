using System;
using System.ComponentModel.DataAnnotations.Schema;
using MeetAndPlay.Data.Models.Files;

namespace MeetAndPlay.Data.Models.Commons
{
    [NotMapped]
    public class FileWithCompressedCopy
    {
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
        public Guid CompressedFileId { get; set; }
        public virtual File CompressedFile { get; set; }
    }
}