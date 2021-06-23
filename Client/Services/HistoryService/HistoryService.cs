using System.Threading.Tasks;
using Microsoft.JSInterop;
using Order.Shared.Contracts;

namespace Order.Client.Services
{
    public class HistoryService : IHistoryService, IScopedService
    {
        private readonly IJSRuntime jsRuntime;

        public HistoryService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask GoBack() => jsRuntime.InvokeVoidAsync("window.history.back");
    }
}
