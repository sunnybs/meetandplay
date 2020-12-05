using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Components
{
    public class AddOfferButtonComponent : ComponentBase
    {
        [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected ElementReference Modal;

        protected async Task ShowAddOfferModalAsync()
        {
            /*
            var httpContext = HttpContextAccessor.HttpContext;
            if (httpContext?.User.Identity != null && !httpContext.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/auth");
                return;
            }
            */
            await JsRuntime.InvokeVoidAsync("showModal", Modal);
        } 
    }
}