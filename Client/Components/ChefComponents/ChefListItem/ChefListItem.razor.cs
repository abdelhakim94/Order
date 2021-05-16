using Microsoft.AspNetCore.Components;
using Order.Shared.Dto.Chef;

namespace Order.Client.Components.Chef
{
    public partial class ChefListItem : ComponentBase
    {
        private string pictureUrl { get => $"background-image:url({Chef?.Picture})"; }

        [Parameter]
        public ChefDetailsDto Chef { get; set; }
    }
}
