using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services.ReadService;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.ReadFilters;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class UserOfferService : IReadService<UserOffer>
    {
        private readonly MNPContext _mnpContext;

        public UserOfferService(MNPContext mnpContext)
        {
            _mnpContext = mnpContext;
        }
        
        public async Task<UserOffer> GetByIdAsync(Guid id)
        {
            var offer = await _mnpContext.UserOffers
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

        public async Task<IReadOnlyList<UserOffer>> GetAsync(ReadFilter filter)
        {
            var userOffers = _mnpContext.UserOffers.AsNoTracking().AsQueryable();
            if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
                userOffers = userOffers.TakePage(filter.PageSize.Value, filter.PageNumber.Value);

            return await userOffers.ToArrayAsync();
        }

        public async Task<CountArray<UserOffer>> GetAsyncAsCountArray(ReadFilter filter)
        {
            var userOffers = _mnpContext.UserOffers.AsNoTracking().AsQueryable();
            var count = await userOffers.CountAsync();
            
            if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
                userOffers = userOffers.TakePage(filter.PageSize.Value, filter.PageNumber.Value);
            
            var resultOffers = await userOffers.ToArrayAsync();
            
            return new CountArray<UserOffer>(resultOffers, count);
        }

        /*
        public async Task<Guid> AddUserOfferAsync(UserOffer userOffer)
        {
            
        }
        
        public async Task<Guid> AddLobbyAsync(Lobby lobby)
        {
            lobby.CreationDate = DateTime.Now;
            lobby.IsActive = true;

            var currentUserId = await _userService.GetCurrentUserIdAsync();
            lobby.LobbyPlayers = new List<LobbyPlayer> {new() {PlayerId = currentUserId, IsCreator = true}};
            
            await _mnpContext.AddAsync(lobby);
            await _mnpContext.SaveChangesAsync();
            return lobby.Id;
        }

        public async Task<Guid> UpdateLobbyAsync(Lobby lobby)
        {
            var oldLobby = await _mnpContext.Lobbies
                .Include(l => l.LobbyPlayers)
                .ThenInclude(lp => lp.Player)
                .AsNoTracking()
                .FindByIdAsync(lobby.Id);
            if (oldLobby == null)
                throw new NotFoundException($"Lobby with {lobby.Id} not found");
            
            lobby.LobbyPlayers = oldLobby.LobbyPlayers;
            await EnsureCurrentUserHasAccessToWriteAsync(lobby);

            var lobbyGames = lobby.LobbyGames;
            lobby.LobbyGames = null;
            _mnpContext.Update(lobby);

            var oldLobbyGames = _mnpContext.LobbyGames
                .Where(lg => lg.LobbyId == lobby.Id);
            _mnpContext.RemoveRange(oldLobbyGames);
            await _mnpContext.AddRangeAsync(lobbyGames);

            await _mnpContext.SaveChangesAsync();
            return lobby.Id;
        }
        */
    }
}