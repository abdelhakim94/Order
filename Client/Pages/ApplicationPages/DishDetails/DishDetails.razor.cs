using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Shared.Dto.Dish;

namespace Order.Client.Pages
{
    public partial class DishDetails : ComponentBase
    {
        Spinner spinner;

        bool OptionsUnfolded = true;
        bool ExtrasUnfolded = true;

        DishDetailsDto dish { get; set; }
        int quantity { get; set; } = 1;

        string pictureUrl { get => $"background-image:url({dish?.Picture})"; }

        HashSet<int> SelectedOptions = new();
        HashSet<int> SelectedExtras = new();

        [Parameter]
        public int Id { get; set; }

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
            // Entering the details page should be done from a list page.
            if (firstRender)
            {
                await GetDishDetails();
                StateHasChanged();
            }
        }

        async Task GetDishDetails()
        {
            spinner?.Show();
            dish = await HubConnection.Invoke<DishDetailsDto, int>("GetDishDetails", Id, Toast);
            spinner?.Hide();
        }

        void OnSelectedOption(int id) => SelectedOptions.Add(id);
        void OnUnselectedOption(int id) => SelectedOptions.Remove(id);
        void OnSelectedExtra(int id) => SelectedExtras.Add(id);
        void OnUnselectedExtra(int id) => SelectedExtras.Remove(id);

        void OnChefDataClick()
        {
            NavigationManager.NavigateTo($"ChefDetails/{dish?.ChefId}");
        }
    }
}
