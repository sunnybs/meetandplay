using System;
using System.Collections.Generic;
using MeetAndPlay.Data.Models.Users;

namespace MeetAndPlay.Data.Models.Offers
{
    /// <summary>
    ///     Анкета конкретного игрока, ищущего возможность сыграть. Отображаются в поиске игроков.
    /// </summary>
    public class UserOffer : BaseEntity
    {
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsConstantSearching { get; set; }

        /// <summary>
        ///     Вариативное поле. Если игрок не находится в постоянном поиске, то анкета будет показана единоразово в указанный
        ///     период
        /// </summary>
        public DateTime ActualOfferDate { get; set; }

        public ICollection<UserOfferPeriod> Periods { get; set; }
        public bool IsActive { get; set; }
    }
}