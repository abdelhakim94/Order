using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Order.Shared.Dto.Dish;

namespace Order.Client.Components.Dish
{
    public partial class DishOrMenuListItem : ComponentBase
    {
        private string pictureUrl { get => $"background-image:url({DishOrMenu?.Picture})"; }

        [Parameter]
        public DishOrMenuListItemDto DishOrMenu { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
