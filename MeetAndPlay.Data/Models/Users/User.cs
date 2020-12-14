using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.AspNetCore.Identity;

namespace MeetAndPlay.Data.Models.Users
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string About { get; set; }
        public ICollection<UserOffer> UserOffers { get; set; }
        public ICollection<LobbyPlayer> UserLobbies { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<UserImage> UserImages { get; set; }
        public ICollection<UserSocialNetwork> UserSocialNetworks { get; set; }
        public ICollection<UserFreePeriod> UserFreePeriods { get; set; }
    }
}
