using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Core.Infrastructure.Extensions;
using MeetAndPlay.Data.Enums;
using MeetAndPlay.Data.Models.Commons;
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
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected SweetAlertService Swal { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        
        protected async Task SubmitAsync()
        {
            if (IsUpdating)
                await UpdateAsync();
            else
                await AddAsync();
        }
        
        protected override async Task OnInitializedAsync()
        {
            if (Id != null && Guid.TryParse(Id, out var parsedId))
            {
                var offer = await UserOfferService.GetByIdAsync(parsedId);
                
                UserOfferModel = Mapper.Map<AddPersonalViewModel>(offer);
                UserOfferModel.Games = offer.UserOfferGames.Select(lg => lg.Game).ToHashSet();
                UserOfferModel.PeriodViewModel = MapPeriodViewModelFromDomain(offer);
            }
            else
            {
                var currentUserId = await UserService.GetCurrentUserIdAsync();
                var alreadyCreatedOffer = await UserOfferService.GetOfferByUserIdAsync(currentUserId);
                if (alreadyCreatedOffer != null)
                {
                    NavigationManager.NavigateTo("/Offer/" + alreadyCreatedOffer.Id, true);
                }
            }
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
                var personalId = await UserOfferService.AddUserOfferAsync(userOffer);
                await Swal.FireAsync("Успешно сохранено!", null, SweetAlertIcon.Success);
                NavigationManager.NavigateTo($"/Offer/{personalId}");
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
            {
                userOffer.Periods = MapPeriodsFromViewModel(UserOfferModel.PeriodViewModel);
                foreach (var period in userOffer.Periods)
                {
                    period.UserOfferId = userOffer.Id;
                }
            }
                
            
            try
            {
                var personalId = await UserOfferService.UpdateUserOfferAsync(userOffer);
                await Swal.FireAsync("Успешно сохранено!", null, SweetAlertIcon.Success);
                NavigationManager.NavigateTo($"/Offer/{personalId}");
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
                    var weekDay = MapFromDescription(day);
                    //TODO: set real hour values
                    results.Add(new UserOfferPeriod {IsDayOfWeek = true, Day = weekDay, HoursFrom = 0, HoursTo = 24});
                }

                return results.ToArray();
            }

            throw new ArgumentException("invalid view model period");
        }

        private WeekDays MapFromDescription(string source)
        {
            return source switch
            {
                "Понедельник" => WeekDays.Monday,
                "Вторник" => WeekDays.Tuesday,
                "Среда" => WeekDays.Wednesday,
                "Четверг" => WeekDays.Thursday,
                "Пятница" => WeekDays.Friday,
                "Суббота" => WeekDays.Saturday,
                "Воскресенье" => WeekDays.Sunday,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            };
        }

        private PeriodViewModel MapPeriodViewModelFromDomain(UserOffer userOffer)
        {
            var domainPeriods = userOffer.Periods;
            if (domainPeriods.Any(p => p.IsEveryday))
                return new PeriodViewModel {IsEveryday = true};
            if (domainPeriods.Any(p => p.IsDayOfWeek))
            {
                var pm = new PeriodViewModel
                {
                    IsDayOfWeek = true, Days = domainPeriods.Select(p => p.Day.GetDescription()).ToHashSet()
                };
                return pm;
            }

            if (userOffer.ActualOfferDate != default)
            {
                return new PeriodViewModel
                {
                    HasActualTime = true, ActualTime = userOffer.ActualOfferDate
                };
            }

            throw new ArgumentException("invalid domain period");
        }
    }
}