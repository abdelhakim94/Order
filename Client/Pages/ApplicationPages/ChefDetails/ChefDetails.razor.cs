using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Shared.Dto.Chef;

namespace Order.Client.Pages
{
    public partial class ChefDetails : ComponentBase
    {
        Spinner spinner;
        ChefDetailsDto chef;
        string pictureUrl { get => $"background-image:url({chef?.Picture})"; }

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Search { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MainLayout.DisplayPreviousPage = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await GetChefDetails();
                StateHasChanged();
            }
        }

        async Task GetChefDetails()
        {
            spinner?.Show();
            chef = await HubConnection.Invoke<ChefDetailsDto, int>("GetChefDetails", Id, Toast);
            spinner?.Hide();
        }

        void NavigateToDishOrMenuDetails(int id, bool isMenu)
        {
            var segment = isMenu ? "MenuDetails" : "DishDetails";
            NavigationManager.NavigateTo($"{segment}/{id}");
        }
    }
}
