using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;

namespace MeetAndPlay.Core.Services
{
    public class OfferAggregator : IOfferAggregator
    {
        public Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            var offers = new List<AggregatedOfferDto>
            {
                new(Guid.Empty,
                    OfferType.Personal,
                    "Тестовый человек, 21 год",
                    "Хочу поиграть в манчкин",
                    "https://www.meme-arsenal.com/memes/1901cf2fa43eac1dba8a005972a6d359.jpg",
                    null,
                    DateTime.Today),
                new(Guid.Empty,
                    OfferType.Lobby,
                    "Собираем команду",
                    "Нас 4, нужно еще 3",
                    "https://minutes.co/wp-content/uploads/2019/04/shutterstock_1214730637.png",
                    null,
                    DateTime.Today),
                new(Guid.Empty,
                    OfferType.Event,
                    "Турнир по манчкину",
                    "Какое то описание",
                    "https://static-ru.insales.ru/images/articles/1/3130/150586/munchkin1.jpg?1504076588",
                    null,
                    DateTime.Today),
                new(Guid.Empty,
                    OfferType.Place,
                    "Антикафе Какое-то",
                    "У нас есть мафия, и много чего еще. Работаем круглосуточно",
                    "https://media-cdn.tripadvisor.com/media/photo-s/15/27/3b/77/caption.jpg",
                    null,
                    DateTime.Today),
            };
            return Task.FromResult(new CountArray<AggregatedOfferDto>(offers.ToArray(), offers.Count));
        }
    }
}