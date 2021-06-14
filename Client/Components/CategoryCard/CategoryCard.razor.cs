using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class CategoryCard : ComponentBase
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Picture { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
