using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Button : ComponentBase
    {
        [Parameter]
        public string Type { get; set; } = "button";

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public bool Danger { get; set; }

        [Parameter]
        public bool Neutral { get; set; }

        private string dangerButton { get => Danger ? "danger-button" : string.Empty; }
        private string neutralButton { get => Neutral ? "neutral-button" : string.Empty; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
