using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MeetAndPlay.Web.Services
{
    public class JSHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public JSHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task AddClassAsync(ElementReference element, string className)
        {
            await _jsRuntime.InvokeVoidAsync("addClass", element, className);
        }
        
        public async Task RemoveClassAsync(ElementReference element, string className)
        {
            await _jsRuntime.InvokeVoidAsync("removeClass", element, className);
        }
        
        public async Task ToggleClassAsync(ElementReference element, string className)
        {
            await _jsRuntime.InvokeVoidAsync("toggleClass", element, className);
        }
        
        public async Task<bool> ContainsClassAsync(ElementReference element, string className)
        {
            return await _jsRuntime.InvokeAsync<bool>("containsClass", element, className);
        }
    }
}