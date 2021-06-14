using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Order.Shared.Dto.Chef;

namespace Order.Client.Components
{
    public partial class ChefListItem : ComponentBase
    {
        private string pictureUrl { get => $"background-image:url({Chef?.Picture})"; }

        [Parameter]
        public ChefListItemDto Chef { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
