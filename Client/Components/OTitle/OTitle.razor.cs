using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class OTitle : ComponentBase
    {
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public double Size { get; set; }

        string sizeStyle { get => $"font-size: {Size}em"; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
