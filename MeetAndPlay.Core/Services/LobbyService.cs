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
        private readonly IUserService _userService;
        private readonly DbContextFactory _contextFactory;
        private readonly IChatService _chatService;

        public LobbyService(IUserService userService, DbContextFactory contextFactory, IChatService chatService)
        {
            _userService = userService;
            _contextFactory = contextFactory;
            _chatService = chatService;
        }

        public async Task<Lobby> GetLobbyByIdAsync(Guid id)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var lobby = await mnpContext.Lobbies
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
            await using var mnpContext = _contextFactory.Create();
            
            lobby.CreationDate = DateTime.Now;
            lobby.IsActive = true;

            var currentUserId = await _userService.GetCurrentUserIdAsync();
            lobby.LobbyPlayers = new List<LobbyPlayer> {new() {PlayerId = currentUserId, IsCreator = true}};
            await mnpContext.AddAsync(lobby);
            
            await mnpContext.SaveChangesAsync();

            var createdLobby = await GetLobbyByIdAsync(lobby.Id);
            var lobbyChatId = await _chatService.CreateChatAsync(createdLobby.Title ?? createdLobby.BuildTitle(), false);
            lobby.ChatId = lobbyChatId;
            await mnpContext.SaveChangesAsync();
            
            return lobby.Id;
        }

        public async Task<Guid> UpdateLobbyAsync(Lobby lobby)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var oldLobby = await mnpContext.Lobbies.AsNoTracking()
                .Include(l => l.LobbyPlayers)
                .ThenInclude(lp => lp.Player).AsNoTracking()
                .FindByIdAsync(lobby.Id);
            if (oldLobby == null)
                throw new NotFoundException($"Lobby with {lobby.Id} not found");

            lobby.LobbyPlayers = oldLobby.LobbyPlayers;
            await EnsureCurrentUserHasAccessToWriteAsync(lobby);

            var lobbyGames = lobby.LobbyGames;
            lobby.LobbyGames = null;
            lobby.CreationDate = oldLobby.CreationDate;
            lobby.IsActive = oldLobby.IsActive;
            lobby.ChatId = oldLobby.ChatId;
            
            mnpContext.Update(lobby);

            var oldLobbyGames = mnpContext.LobbyGames
                .Where(lg => lg.LobbyId == lobby.Id);
            mnpContext.RemoveRange(oldLobbyGames);
            await mnpContext.AddRangeAsync(lobbyGames);
            await mnpContext.SaveChangesAsync();
            return lobby.Id;
        }

        public async Task UpdateLobbyImagesAsync(Guid lobbyId, LobbyImage[] newLobbyImages)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var oldImages = mnpContext.LobbyImages.Where(i => i.LobbyId == lobbyId);
            foreach (var newLobbyImage in newLobbyImages) newLobbyImage.LobbyId = lobbyId;
            mnpContext.LobbyImages.RemoveRange(oldImages);
            await mnpContext.LobbyImages.AddRangeAsync(newLobbyImages);
            await mnpContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserAlreadyRequestedToLobbyAsync(Guid lobbyId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext
                .LobbyJoiningRequests
                .AnyAsync(r => r.UserId == userId && r.LobbyId == lobbyId);
        }

        public async Task AddJoiningRequestAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            await using var mnpContext = _contextFactory.Create();
            
            lobbyJoiningRequest.InitialDate = DateTime.Now;
            lobbyJoiningRequest.RequestStatus = RequestStatus.Initial;
            if (lobbyJoiningRequest.RequestInitiator == RequestInitiator.User)
                lobbyJoiningRequest.UserId = await _userService.GetCurrentUserIdAsync();

            await mnpContext.LobbyJoiningRequests.AddAsync(lobbyJoiningRequest);
            await mnpContext.SaveChangesAsync();
        }

        public async Task<LobbyJoiningRequest[]> GetUserJoiningRequestsAsync(Guid userId, RequestInitiator requestInitiator)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext
                .LobbyJoiningRequests
                .Include(r => r.User)
                .Include(r => r.Lobby)
                .ThenInclude(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Where(r => r.UserId == userId && r.RequestInitiator == requestInitiator)
                .AsNoTracking()
                .ToArrayAsync();
        }
        
        public async Task<LobbyJoiningRequest[]> GetUserLobbiesJoiningRequestsAsync(Guid userId, RequestInitiator requestInitiator)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext
                .LobbyJoiningRequests
                .Include(r => r.User)
                .Include(r => r.Lobby)
                .ThenInclude(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Where(r => r.Lobby.LobbyPlayers
                    .Any(p => p.PlayerId == userId && p.IsCreator) && r.RequestInitiator == requestInitiator)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task RemoveJoiningRequestAsync(Guid lobbyId, Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var request = await mnpContext.LobbyJoiningRequests.FindAsync(new {userId, lobbyId});
            await EnsureRequestInitializedByCurrentUserAsync(request);
            mnpContext.Remove(request);
            await mnpContext.SaveChangesAsync();
        }

        public async Task UpdateJoiningRequestMessageAsync(Guid lobbyId, Guid userId, string newMessage)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var request = await mnpContext.LobbyJoiningRequests.FindAsync(new {userId, lobbyId});
            await EnsureRequestInitializedByCurrentUserAsync(request);

            request.InitialMessage = newMessage;
            await mnpContext.SaveChangesAsync();
        }

        public async Task UpdateJoiningRequestStatus(Guid lobbyId, Guid userId, RequestStatus requestStatus)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var request = await mnpContext.LobbyJoiningRequests.FindAsync(userId, lobbyId);
            await EnsureCurrentUserCanChangeRequestStatusAsync(request);
            request.RequestStatus = requestStatus;
            if (requestStatus == RequestStatus.Accepted
                && !await mnpContext.LobbyPlayers.AnyAsync(lp => lp.LobbyId == lobbyId && lp.PlayerId == userId))
            {
                await mnpContext.AddAsync(new LobbyPlayer {LobbyId = lobbyId, PlayerId = userId, IsCreator = false});
                var lobby = await mnpContext.Lobbies
                    .Include(l => l.LobbyGames)
                    .ThenInclude(l => l.Game)
                    .FindByIdAsync(lobbyId);
                
                lobby.CurrentPlayersCount++;
                await mnpContext.SaveChangesAsync();
                if (lobby.ChatId.HasValue)
                {
                    await _chatService.AddUserToChatAsync(lobby.ChatId.Value, userId);
                    await _chatService.UpdateChatTitleAsync(lobby.ChatId.Value, lobby.Title ?? lobby.BuildTitle());
                }
                    
                return;
            }
            await mnpContext.SaveChangesAsync();
        }

        public async Task<Lobby[]> GetLobbiesCreatedByUserAsync(string userName)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext.Lobbies
                .Include(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Where(l => l.LobbyPlayers.Any(lp => lp.Player.UserName.ToLower() == userName.ToLower() 
                                                     && lp.IsCreator))
                .AsNoTracking()
                .ToArrayAsync();

        }
        
        public async Task<Lobby[]> GetLobbiesCreatedByUserAsync(Guid userId)
        {
            await using var mnpContext = _contextFactory.Create();
            
            return await mnpContext.Lobbies
                .Include(l => l.LobbyGames)
                .ThenInclude(lg => lg.Game)
                .Where(l => l.LobbyPlayers.Any(lp => lp.PlayerId == userId
                                                     && lp.IsCreator))
                .AsNoTracking()
                .ToArrayAsync();
        }
        
        public async Task<CountArray<AggregatedOfferDto>> AggregateOffersAsync(OffersFilterDto filter)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var lobbiesQuery = mnpContext.Lobbies.AsQueryable();
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
                    .Where(l => l.LobbyGames.Any(lg => lg.Game.Name.ToLower().Contains(filter.GameName.ToLower())));

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

        private async Task EnsureCurrentUserCanChangeRequestStatusAsync(LobbyJoiningRequest lobbyJoiningRequest)
        {
            await using var mnpContext = _contextFactory.Create();
            
            var currentUserId = await _userService.GetCurrentUserIdAsync();
            switch (lobbyJoiningRequest.RequestInitiator)
            {
                case RequestInitiator.User:
                {
                    var isCurrentUserLobbyPlayer = await mnpContext.LobbyJoiningRequests.AnyAsync(r =>
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
    }
}