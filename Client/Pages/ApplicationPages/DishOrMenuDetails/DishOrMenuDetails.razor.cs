using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Layouts;

namespace Order.Client.Pages
{
    public partial class DishOrMenuDetails : ComponentBase
    {
        [Parameter]
        public bool IsMenu { get; set; }

        [Parameter]
        public int Id { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Entering the details page should be done from another page
            if (string.IsNullOrWhiteSpace(MainLayout?.PreviousPage))
            {
                NavigationManager.NavigateTo("search/");
            }
        }
    }
}
