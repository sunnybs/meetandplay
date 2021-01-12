using System;
using System.Threading.Tasks;
using MeetAndPlay.Data.DTO.OfferAggregator;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Files;
using MeetAndPlay.Data.Models.Offers;

namespace MeetAndPlay.Core.Abstraction.Services
{
    public interface ILobbyService : IOfferAggregator
    {
        Task<Lobby> GetLobbyByIdAsync(Guid id);
        Task<Guid> AddLobbyAsync(Lobby lobby);
        Task<Guid> UpdateLobbyAsync(Lobby lobby);
        Task UpdateLobbyImagesAsync(Guid lobbyId, LobbyImage[] newLobbyImages);
        Task AddJoiningRequestAsync(LobbyJoiningRequest lobbyJoiningRequest);
        Task RemoveJoiningRequestAsync(Guid lobbyId, Guid userId);
        Task UpdateJoiningRequestMessageAsync(Guid lobbyId, Guid userId, string newMessage);
        Task UpdateJoiningRequestStatus(Guid lobbyId, Guid userId, RequestStatus requestStatus);
    }
}