using Newtonsoft.Json;

namespace MeetAndPlay.Data.DTO.Files
{
    [JsonObject]
    public class Base64UploadRequest
    {
        public string Base64Source { get; set; }
        public string Filename { get; set; }
    }
}