using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Data.Models.Offers;
using MeetAndPlay.Web.Services;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MeetAndPlay.Web.Pages
{
    public class AddLobbyComponent : ComponentBase
    {
        protected ICollection<FileWithCompressedCopy> LobbyInitialImage = new List<FileWithCompressedCopy>();
        protected AddLobbyViewModel LobbyModel = new();
        [Parameter] public string Id { get; set; }
        [Inject] protected ILobbyService LobbyService { get; set; }
        [Inject] protected IGamesService GamesService { get; set; }
        [Inject] protected IMapper Mapper { get; set; }
        [Inject] protected SweetAlertService Swal { get; set; }
        [Inject] protected IApiClient ApiClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected bool IsUpdating => Id != null;
        protected string PageName => IsUpdating ? "Обновить лобби" : "Создать лобби";
        protected string SubmitName => IsUpdating ? "Обновить" : "Создать";

        protected override async Task OnInitializedAsync()
        {
            if (Id != null && Guid.TryParse(Id, out var parsedId))
            {
                var lobby = await LobbyService.GetLobbyByIdAsync(parsedId);
                
                LobbyModel = Mapper.Map<AddLobbyViewModel>(lobby);
                LobbyModel.Games = lobby.LobbyGames.Select(lg => lg.Game).ToHashSet();
                
                LobbyInitialImage = lobby.LobbyImages.Cast<FileWithCompressedCopy>().ToList();
            }
            LobbyModel.CompressedPoster = new List<FileViewModel>();
            LobbyModel.Poster = new List<FileViewModel>();
        }

        protected async Task SubmitAsync()
        {
            if (IsUpdating)
                await UpdateAsync();
            else
                await AddAsync();
        }

        protected async Task AddAsync()
        {
            var lobby = Mapper.Map<Lobby>(LobbyModel);
            lobby.LobbyGames = LobbyModel.Games.Select(g => new LobbyGame {GameId = g.Id}).ToList();
            try
            {
                var lobbyId = await LobbyService.AddLobbyAsync(lobby);
                await UpdatePosterAsync(lobbyId);
                await Swal.FireAsync("Успешно сохранено!", null, SweetAlertIcon.Success);
                NavigationManager.NavigateTo($"/Lobby/{lobbyId}");
            }
            catch (Exception e)
            {
                await Swal.FireAsync("Что-то пошло не так :(", e.Message, SweetAlertIcon.Error);
                throw;
            }
        }

        protected async Task UpdateAsync()
        {
            var parsedId = Guid.Parse(Id);
            var lobby = Mapper.Map<Lobby>(LobbyModel);
            lobby.Id = parsedId;
            lobby.LobbyGames = LobbyModel.Games.Select(g => new LobbyGame {LobbyId = parsedId, GameId = g.Id})
                .ToList();

            try
            {
                var lobbyId = await LobbyService.UpdateLobbyAsync(lobby);
                await UpdatePosterAsync(lobbyId);
                await Swal.FireAsync("Успешно сохранено!", null, SweetAlertIcon.Success);
                NavigationManager.NavigateTo($"/Lobby/{lobbyId}");
            }
            catch (Exception e)
            {
                await Swal.FireAsync("Что-то пошло не так :(", e.Message, SweetAlertIcon.Error);
                throw;
            }
        }
        
        private async Task UpdatePosterAsync(Guid lobbyId)
        {
            var newPoster = LobbyModel.Poster.FirstOrDefault(p => p.IsNewFile);
            var newCompressedPoster = LobbyModel.CompressedPoster.FirstOrDefault(p => p.IsNewFile);
            var lobbyImagesUpdate = new List<LobbyImage>();
            if (newPoster != null && newCompressedPoster != null)
            {
                var newPosterFile = await ApiClient.UploadFileAsync(newPoster.FileLink, newPoster.Filename);
                var newCompressedFile =
                    await ApiClient.UploadFileAsync(newCompressedPoster.FileLink, newCompressedPoster.Filename);
                var lobbyImageDomain = new LobbyImage
                {
                    LobbyId = lobbyId,
                    FileId = newPosterFile.Id,
                    CompressedFileId = newCompressedFile.Id,
                    IsCurrentPoster = true
                };
                lobbyImagesUpdate.Add(lobbyImageDomain);
            }
            await LobbyService.UpdateLobbyImagesAsync(lobbyId, lobbyImagesUpdate.ToArray());
        }
    }
}