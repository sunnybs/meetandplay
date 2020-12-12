using System;

namespace MeetAndPlay.Data.Models.Users
{
    public class UserSocialNetwork : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid SocialNetworkId { get; set; }
        public virtual SocialNetwork SocialNetwork { get; set; }
        /// <summary>
        /// Логин пользователя в соц сети
        /// </summary>
        public string Profile { get; set; }
    }
}