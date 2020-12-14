using System;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure;
using MeetAndPlay.Core.Infrastructure.Exceptions;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Offers;
using Microsoft.EntityFrameworkCore;

namespace MeetAndPlay.Core.Services
{
    public class LobbyService
    {
        private readonly MNPContext _mnpContext;
        private readonly IUserService _userService;

        public LobbyService(MNPContext mnpContext, IUserService userService)
        {
            _mnpContext = mnpContext;
            _userService = userService;
        }

        public async Task<AggregatedOfferDto[]> GetLobbiesForAggregatingAsync(OffersFilterDto filter)
        {
            var lobbiesQuery = _mnpContext.Lobbies
                .Include(l => l.LobbyImages)
                .ThenInclude(i => i.File)
                .AsQueryable();

            if (filter.From.HasValue)
                lobbiesQuery = lobbiesQuery.Where(l => l.PlannedGameDate >= filter.From.Value);
            
            if (filter.To.HasValue)
                lobbiesQuery = lobbiesQuery.Where(l => l.PlannedGameDate <= filter.From.Value);
            
            //TODO: Implement other filters

            var results = await lobbiesQuery.Select(l => new AggregatedOfferDto(
                    l.Id, 
                    OfferType.Lobby, 
                    l.Title, 
                    l.Description,
                    l.LobbyImages.FirstOrDefault(i => i.IsCurrentPoster).File.FileLink, 
                    null, 
                    l.PlannedGameDate))
                .ToArrayAsync();

            return results;
        }

        public async Task<Lobby> GetLobbyByIdAsync(Guid id)
        {
            var lobby = await _mnpContext.Lobbies
                .Include(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Include(l => l.LobbyImages)
                .ThenInclude(i => i.File)
                .Include(l => l.LobbyPlayers)
                .ThenInclude(p => p.Player)
                .FindByIdAsync(id);

            if (lobby == null)
                throw new NotFoundException($"Lobby with {id} not found");

            return lobby;
        }
        
        public async Task<Guid> AddLobbyAsync(Lobby lobby)
        {
            lobby.CreationDate = DateTime.Now;
            lobby.IsActive = true;
            await _mnpContext.AddAsync(lobby);
            await _mnpContext.SaveChangesAsync();
            return lobby.Id;
        }

        public async Task<Guid> UpdateLobbyAsync(Lobby lobby)
        {
            var oldLobby = await _mnpContext.Lobbies
                .Include(l => l.LobbyGames)
                .Include(l => l.LobbyPlayers)
                .AsNoTracking()
                .FindByIdAsync(lobby.Id);

            if (oldLobby == null)
                throw new NotFoundException($"Lobby with {lobby.Id} not found");

            await EnsureCurrentUserHasAccessToWriteAsync(lobby);
            _mnpContext.Update(lobby);
            await _mnpContext.UpdateRelatedEntitiesAsync(lobby.LobbyGames, oldLobby.LobbyGames);
            await _mnpContext.UpdateRelatedEntitiesAsync(lobby.LobbyPlayers, oldLobby.LobbyPlayers);

            await _mnpContext.SaveChangesAsync();
            return lobby.Id;
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
            await _mnpContext.SaveChangesAsync();
        }

        private async Task EnsureCurrentUserCanChangeRequestStatusAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            var isCurrentUserLobbyPlayer = await _mnpContext.LobbyJoiningRequests.AnyAsync(r =>
                r.Lobby.LobbyPlayers.Select(p => p.PlayerId).Contains(currentUserId));

            if (!isCurrentUserLobbyPlayer)
                throw new NoAccessException($"User {currentUserId} can't change lobby request status");
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
    }
}