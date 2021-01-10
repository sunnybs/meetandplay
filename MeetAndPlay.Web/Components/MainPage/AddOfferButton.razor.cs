using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Components
{
    public class AddOfferButtonComponent : ComponentBase
    {
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected ElementReference Modal;

        protected async Task ShowAddOfferModalAsync()
        {
            await JsRuntime.InvokeVoidAsync("showModal", Modal);
        }

        protected async Task RedirectWithCloseModalAsync(string redirectUrl)
        {
            await JsRuntime.InvokeVoidAsync("closeModal", Modal);
            NavigationManager.NavigateTo(redirectUrl);
        }
    }
}