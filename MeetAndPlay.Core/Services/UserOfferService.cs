using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class UserOfferService : IUserOfferService
    {
        private readonly IUserService _userService;
        private readonly DbContextFactory _contextFactory;

        public UserOfferService(IUserService userService, DbContextFactory contextFactory)
        {
            _userService = userService;
            _contextFactory = contextFactory;
        }

        public async Task<UserOffer> GetByIdAsync(Guid id)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var offer = await mnpContext.UserOffers
                .Include(o => o.Author)
                .ThenInclude(a => a.UserImages)
                .ThenInclude(ui => ui.File)

                .Include(o => o.Author)
                .ThenInclude(a => a.UserImages)
                .ThenInclude(ui => ui.CompressedFile)

                .Include(o => o.Periods)
                .Include(o => o.UserOfferGames)
                .ThenInclude(og => og.Game)
                .AsNoTracking()
                .FindByIdAsync(id);

            if (offer == null)
                throw new NotFoundException($"Offer with {id} not found");

            return offer;
        }

        public async Task<UserOffer> GetByUserNameAsync(string username)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var offer = await mnpContext.UserOffers
                .Include(o => o.Author)
                .ThenInclude(a => a.UserImages)
                .ThenInclude(ui => ui.File)

                .Include(o => o.Author)
                .ThenInclude(a => a.UserImages)
                .ThenInclude(ui => ui.CompressedFile)

                .Include(o => o.Periods)
                .Include(o => o.UserOfferGames)
                .ThenInclude(og => og.Game)
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Author.UserName.ToLower() == username.ToLower());

            if (offer == null)
                throw new NotFoundException($"Offer with username {username} not found");

            return offer;
        }

        public async Task<UserOffer> GetOfferByUserIdAsync(Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext.UserOffers.AsNoTracking().FirstOrDefaultAsync(o => o.AuthorId == userId);
        }

        public async Task<IReadOnlyList<UserOffer>> GetAsync(ReadFilter filter)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var userOffers = mnpContext.UserOffers.AsNoTracking().AsQueryable();
            if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
                userOffers = userOffers.TakePage(filter.PageSize.Value, filter.PageNumber.Value);
            return await userOffers.ToArrayAsync();
        }

        public async Task<CountArray<UserOffer>> GetAsyncAsCountArray(ReadFilter filter)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var userOffers = mnpContext.UserOffers.AsNoTracking().AsQueryable();
            var count = await userOffers.CountAsync();
            
            if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
                userOffers = userOffers.TakePage(filter.PageSize.Value, filter.PageNumber.Value);
            
            var resultOffers = await userOffers.ToArrayAsync();
            
            return new CountArray<UserOffer>(resultOffers, count);
        }

        public async Task<Guid> AddUserOfferAsync(UserOffer userOffer)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var authorId = await _userService.GetCurrentUserIdAsync();
            userOffer.AuthorId = authorId;
            userOffer.IsActive = true;
            userOffer.CreationDate = DateTime.Now;

            await mnpContext.AddAsync(userOffer);
            await mnpContext.SaveChangesAsync();
            return userOffer.Id;
        }

        public async Task<Guid> UpdateUserOfferAsync(UserOffer userOffer)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var oldUserOffer = await mnpContext.UserOffers.AsNoTracking().FindByIdAsync(userOffer.Id);
            if (oldUserOffer == null)
                throw new NotFoundException($"Offer with {userOffer.Id} not found");
            
            userOffer.AuthorId = oldUserOffer.AuthorId;
            userOffer.IsActive = oldUserOffer.IsActive;
            userOffer.CreationDate = oldUserOffer.CreationDate;
            
            var offerGames = userOffer.UserOfferGames;
            var periods = userOffer.Periods;
            
            userOffer.UserOfferGames = null;
            userOffer.Periods = null;
            mnpContext.Update(userOffer);
            
            var oldOfferGames = mnpContext.UserOfferGames
                .Where(lg => lg.UserOfferId == userOffer.Id);
            mnpContext.RemoveRange(oldOfferGames);
            await mnpContext.AddRangeAsync(offerGames);
            
            var oldPeriods = mnpContext.UserOfferPeriods
                .Where(lg => lg.UserOfferId == userOffer.Id);
            mnpContext.RemoveRange(oldPeriods);
            await mnpContext.AddRangeAsync(periods);
            
            await mnpContext.SaveChangesAsync();
            return userOffer.Id;
        }

        public async Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var offerQuery = mnpContext.UserOffers.AsQueryable();
             if (filter.From.HasValue)
             {
                 var filterWeekDay = (WeekDays)((int)(filter.From.Value.DayOfWeek + 6) % 7);
                 var hour = filter.From.Value.Hour;
                 offerQuery = offerQuery.Where(l => 
                     l.Periods.Any(p => p.IsEveryday
                     || l.Periods.Any(p => p.Day == filterWeekDay && p.HoursFrom >= hour)
                     || l.ActualOfferDate >= filter.From.Value));
             }
             
             if (filter.To.HasValue)
             {
                var filterWeekDay = (WeekDays)((int)(filter.To.Value.DayOfWeek + 6) % 7);
                var hour = filter.To.Value.Hour;
                offerQuery = offerQuery.Where(l => 
                    l.Periods.Any(p => p.IsEveryday
                                       || l.Periods.Any(p => p.Day == filterWeekDay && p.HoursFrom <= hour)
                                       || l.ActualOfferDate <= filter.To.Value));
             }

            
            if (filter.PlaceType.HasValue && filter.PlaceType != PlaceType.Undefined)
                offerQuery = offerQuery.Where(l => l.PlaceType == filter.PlaceType);

            if (!filter.GameName.IsNullOrWhiteSpace())
                offerQuery = offerQuery
                    .Where(l => l.UserOfferGames.Any(lg => lg.Game.Name.ToLower().Contains(filter.GameName.ToLower())));

            if (filter.AgeFrom.HasValue)
            {
                var dateFrom = DateTime.Now.AddYears(filter.AgeFrom.Value * -1);
                offerQuery = offerQuery
                    .Where(l => l.Author.BirthDate.Value >= dateFrom);
            }
            
            if (filter.AgeTo.HasValue)
            {
                var dateTo = DateTime.Now.AddYears(filter.AgeTo.Value * -1);
                offerQuery = offerQuery
                    .Where(l => l.Author.BirthDate.Value <= dateTo);
            }

            var count = await offerQuery.CountAsync();

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
                offerQuery = offerQuery.TakePage(filter.PageSize.Value, filter.PageNumber.Value);
            
            var results = await offerQuery.AsNoTracking()
                .Select(l => new AggregatedOfferDto(
                    l.Id,
                    OfferType.Personal,
                    "Хочу поиграть",
                    l.Description,
                    l.Author.UserImages.FirstOrDefault(i => i.IsCurrentAvatar).File.FileLink,
                    null,
                    l.ActualOfferDate,
                    l.UserOfferGames.Select(lg => lg.Game.Name).ToArray(),
                    GameLevel.Undefined,
                    l.PlaceType))
                .ToArrayAsync();

            return new CountArray<AggregatedOfferDto>(results, count);
        }
    }
}