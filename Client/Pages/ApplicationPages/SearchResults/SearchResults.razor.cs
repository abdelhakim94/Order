using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Order.Client.Components;
using Order.Client.Constants;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Server.Dto;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;
using Order.Shared.Dto.Chef;
using Order.Shared.Dto.Dish;

namespace Order.Client.Pages
{
    public partial class SearchResults : ComponentBase, IDisposable
    {
        Spinner spinner;
        ValueWrapperDto<string> SearchValue { get; set; } = new(string.Empty);

        PaginatedList<DishOrMenuListItemDto> dishAndMenues;
        DishesOrMenuesSearchFilter dishAndMenuesFilter;
        int dishAndMenuesPageIndex;

        PaginatedList<ChefListItemDto> chefs;
        ChefsSearchFilter chefsSearchFilter;
        int chefsPageIndex;

        bool searchByChefs;
        UserAddressDetailDto address;

        [Parameter]
        public string Search { get; set; }

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

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        DotNetObjectReference<SearchResults> thisReference;

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
                address = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
                dishAndMenuesPageIndex = 1;
                chefsPageIndex = 1;
                SearchValue.Value = Search;

                var thisReference = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("OnSearchResultEndScroll", thisReference);

                await SearchChefsOrDishesAndMenues();
            }
        }

        async Task HandleDishOrChefToggle(bool value)
        {
            searchByChefs = value;
            await SearchChefsOrDishesAndMenues();
        }

        [JSInvokable("SearchChefsOrDishesAndMenues")]
        public async Task SearchChefsOrDishesAndMenues()
        {
            if (address is null)
            {
                NavigationManager.NavigateTo("search/");
                return;
            }
            if (searchByChefs)
            {
                await SearchChefs();
            }
            else
            {
                await SearchDishesAndMenues();
            }
            StateHasChanged();
        }

        async Task SearchChefs()
        {
            if (chefs is null || chefs.Items.Count < chefs.TotalItems)
            {
                chefsSearchFilter = new()
                {
                    Search = Search,
                    Latitude = address.Latitude,
                    Longitude = address.Longitude,
                    PageIndex = chefsPageIndex++,
                    ItemsPerPage = Pagination.ItemsPerPage,
                };

                spinner?.Show();
                var results = await HubConnection.Invoke<PaginatedList<ChefListItemDto>, ChefsSearchFilter>(
                    "GetChefs",
                    chefsSearchFilter,
                    Toast);
                if (chefs is not null) chefs.AddRange(results.Items, new ChefListItemComparer());
                else chefs = results;
                spinner?.Hide();
            }
        }

        async Task SearchDishesAndMenues()
        {
            if (dishAndMenues is null || dishAndMenues.Items.Count < dishAndMenues.TotalItems)
            {
                dishAndMenuesFilter = new()
                {
                    Search = Search,
                    Latitude = address.Latitude,
                    Longitude = address.Longitude,
                    PageIndex = dishAndMenuesPageIndex++,
                    ItemsPerPage = Pagination.ItemsPerPage,
                };

                spinner?.Show();
                var results = await HubConnection.Invoke<PaginatedList<DishOrMenuListItemDto>, DishesOrMenuesSearchFilter>(
                    "GetDishesAndMenues",
                    dishAndMenuesFilter,
                    Toast);
                if (dishAndMenues is not null) dishAndMenues.AddRange(results.Items, new DishOrMenuListItemEqualityComparer());
                else dishAndMenues = results;
                spinner?.Hide();
            }
        }

        void NavigateToDishOrMenuDetails(DishOrMenuListItemDto item)
        {
            if (item.IsMenu)
            {
                NavigationManager.NavigateTo($"MenuDetails/{item.Id}");
            }
            else
            {
                NavigationManager.NavigateTo($"DishDetails/{item.Id}");
            }
        }

        void NavigateToChefDetails(ChefListItemDto item)
        {
            NavigationManager.NavigateTo($"ChefDetails/{item.Id}");
        }

        async Task HandleSearch(string search)
        {

            if (string.IsNullOrWhiteSpace(search))
                return;

            if (string.IsNullOrWhiteSpace(address?.Address1) || string.IsNullOrWhiteSpace(address?.City))
                return;

            dishAndMenues = null;
            dishAndMenuesPageIndex = 1;
            chefs = null;
            chefsPageIndex = 1;
            Search = search;
            await SearchChefsOrDishesAndMenues();
            NavigationManager.NavigateTo($"search/{SearchValue?.Value}");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (thisReference is not null)
            {
                // TO DO : Remove the listener added to the 'scroll' event in the javascript code.
                thisReference.Dispose();
            }
        }
    }
}
