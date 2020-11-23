using System;
using System.Collections.Generic;

namespace MeetAndPlay.Offers.Data.Models
{
    /// <summary>
    /// Анкета конкретного игрока, ищущего возможность сыграть. Отображаются в поиске игроков.
    /// </summary>
    public class UserOffer
    {
        public Guid UserOfferId { get; set; } 
        public Guid AuthorId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsConstantSearching { get; set; }
        /// <summary>
        /// Вариативное поле. Если игрок не находится в постоянном поиске, то анкета будет показана единоразово в указанный период
        /// </summary>
        public DateTime ActualOfferDate { get; set; }
        public ICollection<UserOfferPeriod> Periods { get; set; }
        public bool IsActive { get; set; }
    }
}