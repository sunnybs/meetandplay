using MeetAndPlay.Data.Interfaces;

namespace MeetAndPlay.Data.Models.Users
{
    public class SocialNetwork : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
        public string PosterLink { get; set; }
        public string BaseUrl { get; set; }
    }
}