using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Files;
using MeetAndPlay.Data.Models.Places;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly MNPContext _mnpContext;

        public PlaceService(MNPContext mnpContext)
        {
            _mnpContext = mnpContext;
        }

        public async Task ExecuteSeeding()
        {
            var country = new Country
            {
                Name = "Россия"
            };

            var city = new City
            {
                Name = "Екатеринбург",
                Country = country
            };

            var places = new List<Place>
            {
                new()
                {
                    Name = "Антикафе «Автограф»",
                    Description =
                        "В антикафе вы платите только за проведённое время: одна минута здесь стоит два рубля. Каждый день здесь происходит что-то интересное и, конечно, не обходится без настольных игр. Тут играют в «Мафию», в Бэнг и устраивают большие игротеки. Впрочем, вы можете прийти и сыграть в любую игру вне расписания. Выбор очень большой!",
                    PlaceType = PlaceType.Cafe
                },
                new()
                {
                    Name = "Антикафе «Коммуникатор»",
                    Description =
                        "Ещё одно антикафе, где всегда царит дружеская и домашняя атмосфера. Много настольных игр, насыщенное расписание, вкусный кофе делают это место очень популярным. Одна минута первого часа стоит два рубля, последующих часов - рубль. Приятно, что можно получить бессрочную карту постоянного гостя, которая даёт скидку на пребывание в заведении.",
                    PlaceType = PlaceType.Cafe
                },
                new()
                {
                    Name = "Вместе Party Place",
                    Description =
                        "Пространство, в котором можно собраться компанией на любой праздник. Здесь не будет шумно и вам никто не помешает послушать музыку, попеть в караоке и поиграть в любимые настольные игры. Можно выбрать один из восьми залов в соответствии с количеством игроков.",
                    PlaceType = PlaceType.Cafe
                }
            };

            var files = new List<File>
            {
                new()
                {
                    CreationDate = DateTime.Now,
                    FileLink =
                        "https://kudago.com/media/thumbs/m/images/place/2d/e7/2de7f29dba4fcb98377690ab9a6d162c.jpg"
                },
                new()
                {
                    CreationDate = DateTime.Now,
                    FileLink =
                        "https://kudago.com/media/thumbs/m/images/place/e7/26/e7269247edfc71ee3f7d8cd7a5f5a0bc.jpg"
                },
                new()
                {
                    CreationDate = DateTime.Now,
                    FileLink =
                        "https://kudago.com/media/thumbs/m/images/list/1f/c8/1fc8722cb7a6b576bba414f1904f6a89.jpg"
                }
            };

            var placeImages = new List<PlaceImage>
            {
                new()
                {
                    IsCurrentPoster = true,
                    Place = places[0],
                    File = files[0],
                    CompressedFile = files[0]
                },
                new()
                {
                    IsCurrentPoster = true,
                    Place = places[1],
                    File = files[1],
                    CompressedFile = files[1]
                },
                new()
                {
                    IsCurrentPoster = true,
                    Place = places[2],
                    File = files[2],
                    CompressedFile = files[2]
                }
            };

            var locations = new List<Location>
            {
                new()
                {
                    Address = "ул. Добролюбова, д. 16",
                    City = city,
                    EndWorkHour = 22,
                    IsActive = true,
                    Place = places[0]
                },
                new()
                {
                    Address = "ул. Тургенева, д. 22",
                    City = city,
                    EndWorkHour = 22,
                    IsActive = true,
                    Place = places[1]
                },
                new()
                {
                    Address = "ул. Репина, д. 22",
                    City = city,
                    EndWorkHour = 22,
                    IsActive = true,
                    Place = places[2]
                }
            };

            await _mnpContext.AddAsync(country);
            await _mnpContext.AddAsync(city);
            await _mnpContext.AddRangeAsync(places);
            await _mnpContext.AddRangeAsync(locations);
            await _mnpContext.AddRangeAsync(files);
            await _mnpContext.AddRangeAsync(placeImages);

            await _mnpContext.SaveAndDetachAsync();
        }

        public async Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            //TODO: implement HasManyGames in place

            var placesQuery = _mnpContext.Places.AsQueryable();

            if (filter.PlaceType.HasValue && filter.PlaceType != PlaceType.Undefined)
                placesQuery = placesQuery.Where(pl => pl.PlaceType == filter.PlaceType);

            var count = await placesQuery.CountAsync();
            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
                placesQuery = placesQuery.TakePage(filter.PageSize.Value, filter.PageNumber.Value);

            var results = await placesQuery.AsNoTracking().Select(p => new AggregatedOfferDto
                (p.Id,
                    OfferType.Place,
                    p.Name,
                    p.Description,
                    p.PlaceImages.FirstOrDefault(i => i.IsCurrentPoster).File.FileLink,
                    null,
                    null,
                    null,
                    GameLevel.Undefined,
                    p.PlaceType))
                .ToArrayAsync();

            return new CountArray<AggregatedOfferDto>(results, count);
        }
    }
}