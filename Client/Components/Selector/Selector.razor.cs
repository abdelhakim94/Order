using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Order.Client.Components
{
    public partial class Selector : ComponentBase
    {
        string selectorArrow
        {
            get => unfolded ? "icons/open-selector-arrow.png" : "icons/closed-selector-arrow.png";
        }

        bool unfolded;
        [Parameter]
        public bool Unfolded
        {
            get => unfolded;
            set
            {
                unfolded = value;
                if (UnfoldedChanged.HasDelegate)
                {
                    UnfoldedChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<bool> UnfoldedChanged { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        void HandleClick(MouseEventArgs args)
        {
            Unfolded = !unfolded;
            StateHasChanged();
        }
    }
}
