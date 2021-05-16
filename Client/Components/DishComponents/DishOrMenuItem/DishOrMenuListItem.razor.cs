using Microsoft.AspNetCore.Components;
using Order.Shared.Dto.Dish;

namespace Order.Client.Components.Dish
{
    public partial class DishOrMenuListItem : ComponentBase
    {
        private string pictureUrl { get => $"background-image:url({DishOrMenu?.Picture})"; }

        [Parameter]
        public DishOrMenuDetailsDto DishOrMenu { get; set; }
    }
}
