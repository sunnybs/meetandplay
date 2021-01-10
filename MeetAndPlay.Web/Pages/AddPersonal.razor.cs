using System;
using System.Threading.Tasks;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using MeetAndPlay.Core.Abstraction.Services;
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
        [Inject] protected SweetAlertService Swal { get; set; }
        
        protected async Task SubmitAsync()
        {
            throw new NotImplementedException();
        }
    }
}