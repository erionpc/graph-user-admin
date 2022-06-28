using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace B2CUserAdmin.UI.Services
{
    // https://www.daveabrock.com/2021/02/18/copy-to-clipboard-markdown-editor/ 
    public class ClipboardService
    {
        private readonly IJSRuntime _jsRuntime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask WriteTextAsync(string text)
        {
            return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}
