using System.Collections.Generic;
using AutoMapper;
using CurrieTechnologies.Razor.SweetAlert2;
using MeetAndPlay.Core.Abstraction.Services;
using MeetAndPlay.Data.Models.Commons;
using MeetAndPlay.Web.Services;
using MeetAndPlay.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MeetAndPlay.Web.Pages
{
    public class EditProfileComponent : ComponentBase
    {
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected IApiClient ApiClient { get; set; }
        [Inject] protected IMapper Mapper { get; set; }
        [Inject] protected SweetAlertService Swal { get; set; }
        
        protected ICollection<FileWithCompressedCopy> AvatarInitialImage = new List<FileWithCompressedCopy>();
        protected EditProfileViewModel ProfileViewModel { get; set; } = new();
    }
}