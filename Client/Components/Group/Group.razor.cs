using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Group : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
