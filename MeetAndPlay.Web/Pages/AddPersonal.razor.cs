using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Offers;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Pages
{
    public class AddPersonalComponent : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        protected AddPersonalViewModel UserOfferModel = new();
        protected bool IsUpdating => Id != null;
        protected string PageName => "Напишите ваши пожелания для игры, чтобы людям было проще Вас найти.";
        protected string SubmitName => IsUpdating ? "Обновить" : "Создать";
        [Inject] protected IGamesService GamesService { get; set; }
        [Inject] protected IMapper Mapper { get; set; }
        [Inject] protected IUserOfferService UserOfferService { get; set; }
        [Inject] protected SweetAlertService Swal { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        
        protected async Task SubmitAsync()
        {
            if (IsUpdating)
                await UpdateAsync();
            else
                await AddAsync();
        }
        
        protected async Task AddAsync()
        {
            var userOffer = Mapper.Map<UserOffer>(UserOfferModel);
            userOffer.UserOfferGames = UserOfferModel.Games.Select(g => new UserOfferGame() {GameId = g.Id}).ToList();
            
            if (UserOfferModel.PeriodViewModel.HasActualTime)
                userOffer.ActualOfferDate = UserOfferModel.PeriodViewModel.ActualTime ?? default;
            else
                userOffer.Periods = MapPeriodsFromViewModel(UserOfferModel.PeriodViewModel);
            
            try
            {
                var lobbyId = await UserOfferService.AddUserOfferAsync(userOffer);
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
            var userOffer = Mapper.Map<UserOffer>(UserOfferModel);
            userOffer.Id = parsedId;
            userOffer.UserOfferGames = UserOfferModel.Games.Select(g => new UserOfferGame {UserOfferId = parsedId, GameId = g.Id})
                .ToList();

            if (UserOfferModel.PeriodViewModel.HasActualTime)
                userOffer.ActualOfferDate = UserOfferModel.PeriodViewModel.ActualTime ?? default;
            else
                userOffer.Periods = MapPeriodsFromViewModel(UserOfferModel.PeriodViewModel);
            
            try
            {
                var lobbyId = await UserOfferService.UpdateUserOfferAsync(userOffer);
                await Swal.FireAsync("Успешно сохранено!", null, SweetAlertIcon.Success);
                NavigationManager.NavigateTo($"/Lobby/{lobbyId}");
            }
            catch (Exception e)
            {
                await Swal.FireAsync("Что-то пошло не так :(", e.Message, SweetAlertIcon.Error);
                throw;
            }
        }

        private UserOfferPeriod[] MapPeriodsFromViewModel(PeriodViewModel period)
        {
            if (period.IsEveryday)
                return new[] {new UserOfferPeriod {IsEveryday = true}};

            if (period.IsDayOfWeek)
            {
                var results = new List<UserOfferPeriod>();
                foreach (var day in period.Days)
                {
                    var weekDay = Enum.Parse<WeekDays>(day);
                    //TODO: set real hour values
                    results.Add(new UserOfferPeriod {IsDayOfWeek = true, Day = weekDay, HoursFrom = 0, HoursTo = 24});
                }

                return results.ToArray();
            }

            throw new ArgumentException();
        }
    }
}