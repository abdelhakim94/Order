using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Redirect : ComponentBase
    {
        [Parameter]
        public string Route { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo(Route);
        }
    }
}
