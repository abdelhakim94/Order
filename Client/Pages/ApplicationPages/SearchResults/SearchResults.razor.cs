using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Server.Dto;
using Order.Shared.Dto.Address;
using Order.Shared.Dto.Chef;
using Order.Shared.Dto.Dish;

namespace Order.Client.Pages
{
    public partial class SearchResults : ComponentBase
    {
        private PaginatedList<DishOrMenuDetailsDto> dishAndMenues;
        private DishesOrMenuesSearchFilter dishAndMenuesFilter;
        private int dishAndMenuesPageIndex;

        private PaginatedList<ChefDetailsDto> chefs;
        private ChefsSearchFilter chefsSearchFilter;
        private int chefsPageIndex;

        private bool searchByChefs;
        private UserAddressDetailDto address;

        private string remaining
        {
            get => searchByChefs
                ? $"{chefs?.TotalItems - chefs?.Items?.Count} {UIMessages.Remaining}"
                : $"{dishAndMenues?.TotalItems - dishAndMenues?.Items?.Count} {UIMessages.Remaining}";
        }

        [Parameter]
        public string Search { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public IStateStore Store { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            MainLayout.PreviousPage = "search/";
            address = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
            dishAndMenuesPageIndex = 1;
            chefsPageIndex = 1;
            await SearchChefsOrDishesAndMenues();
        }

        async Task HandleDishOrChefToggle(bool value)
        {
            if (value && chefs is null)
            {
                await SearchChefs();
            }
            else if (!value && dishAndMenues is null)
            {
                await SearchDishesAndMenues();
            }
            searchByChefs = value;
        }

        async Task SearchDishesAndMenues()
        {
            if (address is null)
            {
                NavigationManager.NavigateTo("search/");
                return;
            }

            dishAndMenuesFilter = new()
            {
                Search = Search,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                PageIndex = dishAndMenuesPageIndex++,
                ItemsPerPage = Pagination.ItemsPerPage,
            };

            Spinner.Show();
            var results = await HubConnection.Invoke<PaginatedList<DishOrMenuDetailsDto>, DishesOrMenuesSearchFilter>(
                "GetDishesAndMenues",
                dishAndMenuesFilter,
                Toast);
            if (dishAndMenues is not null) dishAndMenues.AddRange(results.Items, new DishOrMenuDetailEqualityComparer());
            else dishAndMenues = results;
            Spinner.Hide();
        }

        async Task SearchChefs()
        {
            if (address is null)
            {
                NavigationManager.NavigateTo("search/");
                return;
            }

            chefsSearchFilter = new()
            {
                Search = Search,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                PageIndex = chefsPageIndex++,
                ItemsPerPage = Pagination.ItemsPerPage,
            };

            Spinner.Show();
            var results = await HubConnection.Invoke<PaginatedList<ChefDetailsDto>, ChefsSearchFilter>(
                "GetChefs",
                chefsSearchFilter,
                Toast);
            if (chefs is not null) chefs.AddRange(results.Items, new ChefDetailsComparer());
            else chefs = results;
            Spinner.Hide();
        }

        async Task SearchChefsOrDishesAndMenues()
        {
            if (searchByChefs)
            {
                await SearchChefs();
            }
            else
            {
                await SearchDishesAndMenues();
            }
        }
    }
}
