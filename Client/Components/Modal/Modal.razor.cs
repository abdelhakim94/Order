using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Modal : ComponentBase
    {
        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        private bool ShowModal { get; set; }

        public void Show()
        {
            ShowModal = true;
            StateHasChanged();
        }

        public async Task Close()
        {
            ShowModal = false;
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync();
            }
            StateHasChanged();
        }
    }
}
