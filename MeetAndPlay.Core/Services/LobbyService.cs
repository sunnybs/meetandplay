using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly MNPContext _mnpContext;
        private readonly IUserService _userService;

        public LobbyService(MNPContext mnpContext, IUserService userService)
        {
            _mnpContext = mnpContext;
            _userService = userService;
        }

        public async Task<Lobby> GetLobbyByIdAsync(Guid id)
        {
            var lobby = await _mnpContext.Lobbies
                .Include(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Include(l => l.LobbyImages)
                .ThenInclude(i => i.File)
                .Include(l => l.LobbyImages)
                .ThenInclude(i => i.CompressedFile)
                .Include(l => l.LobbyPlayers)
                .ThenInclude(p => p.Player)
                .AsNoTracking()
                .FindByIdAsync(id);
            if (lobby == null)
                throw new NotFoundException($"Lobby with {id} not found");
            return lobby;
        }

        public async Task<Guid> AddLobbyAsync(Lobby lobby)
        {

                lobby.CreationDate = DateTime.Now;
                lobby.IsActive = true;

                var currentUserId = await _userService.GetCurrentUserIdAsync();
                lobby.LobbyPlayers = new List<LobbyPlayer> {new() {PlayerId = currentUserId, IsCreator = true}};

                await _mnpContext.AddAsync(lobby);
                await _mnpContext.SaveChangesAsync();
                _mnpContext.DetachAll();
                return lobby.Id;
        }

        public async Task<Guid> UpdateLobbyAsync(Lobby lobby)
        {
            var oldLobby = await _mnpContext.Lobbies.AsNoTracking()
                    .Include(l => l.LobbyPlayers)
                    .ThenInclude(lp => lp.Player).AsNoTracking()
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
                _mnpContext.DetachAll();
                return lobby.Id;
        }

        public async Task UpdateLobbyImagesAsync(Guid lobbyId, LobbyImage[] newLobbyImages)
        {
            var oldImages = _mnpContext.LobbyImages.Where(i => i.LobbyId == lobbyId);
            foreach (var newLobbyImage in newLobbyImages) newLobbyImage.LobbyId = lobbyId;
            _mnpContext.LobbyImages.RemoveRange(oldImages);
            await _mnpContext.LobbyImages.AddRangeAsync(newLobbyImages);
            await _mnpContext.SaveChangesAsync();
            _mnpContext.DetachAll();
        }

        public async Task AddJoiningRequestAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            await EnsureRequestInitializedByCurrentUserAsync(lobbyJoiningRequest);
            await _mnpContext.LobbyJoiningRequests.AddAsync(lobbyJoiningRequest);
            await _mnpContext.SaveChangesAsync();
        }

        public async Task RemoveJoiningRequestAsync(Guid lobbyId, Guid userId)
        {
            var request = await _mnpContext.LobbyJoiningRequests.FindAsync(new {userId, lobbyId});
            await EnsureRequestInitializedByCurrentUserAsync(request);
            _mnpContext.Remove(request);
            await _mnpContext.SaveChangesAsync();
        }

        public async Task UpdateJoiningRequestMessageAsync(Guid lobbyId, Guid userId, string newMessage)
        {
            var request = await _mnpContext.LobbyJoiningRequests.FindAsync(new {userId, lobbyId});
            await EnsureRequestInitializedByCurrentUserAsync(request);

            request.InitialMessage = newMessage;
            await _mnpContext.SaveChangesAsync();
        }

        public async Task UpdateJoiningRequestStatus(Guid lobbyId, Guid userId, RequestStatus requestStatus)
        {
            var request = await _mnpContext.LobbyJoiningRequests.FindAsync(new {userId, lobbyId});
            await EnsureCurrentUserCanChangeRequestStatusAsync(request);
            request.RequestStatus = requestStatus;
            if (requestStatus == RequestStatus.Accepted
                && await _mnpContext.LobbyPlayers.AnyAsync(lp => lp.LobbyId == lobbyId && lp.PlayerId == userId))
            {
                await _mnpContext.AddAsync(new LobbyPlayer {LobbyId = lobbyId, PlayerId = userId, IsCreator = false});
                var lobby = await _mnpContext.Lobbies.FindAsync(lobbyId);
                lobby.CurrentPlayersCount++;
            }

            await _mnpContext.SaveChangesAsync();
        }

        private async Task EnsureCurrentUserCanChangeRequestStatusAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            switch (lobbyJoiningRequest.RequestInitiator)
            {
                case RequestInitiator.User:
                {
                    var isCurrentUserLobbyPlayer = await _mnpContext.LobbyJoiningRequests.AnyAsync(r =>
                        r.Lobby.Id == lobbyJoiningRequest.LobbyId
                        && r.Lobby.LobbyPlayers.Select(p => p.PlayerId).Contains(currentUserId));

                    if (!isCurrentUserLobbyPlayer)
                        throw new NoAccessException($"User {currentUserId} can't change lobby request status");
                    break;
                }
                case RequestInitiator.Lobby:
                {
                    if (lobbyJoiningRequest.UserId != currentUserId)
                        throw new NoAccessException($"User {currentUserId} can't change lobby request status");
                    break;
                }
            }
        }

        private async Task EnsureRequestInitializedByCurrentUserAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            if (lobbyJoiningRequest.UserId != currentUserId)
                throw new NoAccessException($"User {currentUserId} can't add joining request with other user id.");
        }

        private async Task EnsureCurrentUserHasAccessToWriteAsync(Lobby lobby)
        {
            var lobbyCreator = lobby.LobbyPlayers.Single(p => p.IsCreator).Player;
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            if (lobbyCreator.Id != currentUserId)
                throw new NoAccessException($"User {currentUserId} can't edit lobby {lobby.Id}");
        }

        public async Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            var lobbiesQuery = _mnpContext.Lobbies.AsQueryable();
            if (filter.From.HasValue)
                lobbiesQuery = lobbiesQuery.Where(l => l.PlannedGameDate >= filter.From);
            
            if (filter.To.HasValue)
                lobbiesQuery = lobbiesQuery.Where(l => l.PlannedGameDate >= filter.To);

            if (filter.PlaceType.HasValue && filter.PlaceType != PlaceType.Undefined)
                lobbiesQuery = lobbiesQuery.Where(l => l.PlaceType == filter.PlaceType);
            
            if (filter.GameLevel.HasValue && filter.GameLevel != GameLevel.Undefined)
                lobbiesQuery = lobbiesQuery.Where(l => l.GameLevel == filter.GameLevel);

            if (!filter.GameName.IsNullOrWhiteSpace())
                lobbiesQuery = lobbiesQuery
                    .Where(l => l.LobbyGames.Any(lg => lg.Game.Name.Contains(filter.GameName)));

            if (filter.AgeFrom.HasValue)
            {
                var dateFrom = DateTime.Now.AddYears(filter.AgeFrom.Value * -1);
                lobbiesQuery = lobbiesQuery
                    .Where(l => l.LobbyPlayers.Any(lp => lp.Player.BirthDate.Value >= dateFrom));
            }
            
            if (filter.AgeTo.HasValue)
            {
                var dateTo = DateTime.Now.AddYears(filter.AgeTo.Value * -1);
                lobbiesQuery = lobbiesQuery
                    .Where(l => l.LobbyPlayers.Any(lp => lp.Player.BirthDate.Value <= dateTo));
            }

            var count = await lobbiesQuery.CountAsync();

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
                lobbiesQuery = lobbiesQuery.TakePage(filter.PageSize.Value, filter.PageNumber.Value);
            
            var results = await lobbiesQuery.AsNoTracking()
                .Select(l => new AggregatedOfferDto(
                    l.Id,
                    OfferType.Lobby,
                    l.Title,
                    l.Description,
                    l.LobbyImages.FirstOrDefault(i => i.IsCurrentPoster).File.FileLink,
                    null,
                    l.PlannedGameDate,
                    l.LobbyGames.Select(lg => lg.Game.Name).ToArray(),
                    l.GameLevel,
                    l.PlaceType))
                .ToArrayAsync();

            return new CountArray<AggregatedOfferDto>(results, count);
        }
    }
}