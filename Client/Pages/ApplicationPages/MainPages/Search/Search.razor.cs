using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Constants;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;
using Order.Shared.Dto.Category;

namespace Order.Client.Pages
{
    public partial class Search : ComponentBase
    {
        Spinner categoriesSpinner;
        Spinner addressSpinner;
        ValueWrapperDto<string> SearchValue { get; set; } = new(string.Empty);
        CloneableList<CategoryListItemDto> Categories;
        UserAddressDetailDto CurrentAddress { get; set; }
        AddressModal AddressModal;
        IEnumerable<DatalistOption> options { get; set; } = new List<DatalistOption> { };

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public IStateStore Store { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public MainPagesLayout BottomLayout { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                BottomLayout.SearchSelected = true;
                MainLayout.DisplayPreviousPage = false;

                Task<CloneableList<CategoryListItemDto>> categoriesTask = null;
                Task<UserAddressDetailDto> addressTask = null;

                Categories = Store.Get<CloneableList<CategoryListItemDto>>(StoreKey.CATEGORIES);
                if (Categories is null)
                {
                    categoriesSpinner?.Show();
                    categoriesTask = HubConnection.Invoke<CloneableList<CategoryListItemDto>>("GetCategories", Toast);
                }

                CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
                if (CurrentAddress is null)
                {
                    addressSpinner?.Show();
                    Console.WriteLine("showing spinner");
                    addressTask = HubConnection.Invoke<UserAddressDetailDto>("GetLastUsedAddress", Toast);
                }

                if (categoriesTask is not null)
                {
                    Categories = await categoriesTask;
                    Store.Set(StoreKey.CATEGORIES, Categories);
                    categoriesSpinner?.Hide();
                }

                if (addressTask is not null)
                {
                    CurrentAddress = await addressTask;
                    if (CurrentAddress is not null)
                    {
                        Store.Set(StoreKey.ADDRESS, CurrentAddress);
                    }
                    else
                    {
                        AddressModal.Show();
                    }
                    addressSpinner?.Hide();
                }

                Store.OnUpdate += OnStoreAddressChange;
                StateHasChanged();
            }
        }

        void HandleAddressBarClick()
        {
            AddressModal.Show();
        }

        void OnStoreAddressChange(object sender, StoreUpdateArgs args)
        {
            if (args.Key is StoreKey.ADDRESS)
            {
                CurrentAddress = args.Value as UserAddressDetailDto;
                StateHasChanged();
            }
        }

        void HandleSearch(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return;

            if (string.IsNullOrWhiteSpace(CurrentAddress?.Address1) || string.IsNullOrWhiteSpace(CurrentAddress?.City))
            {
                AddressModal.Show();
                return;
            }

            NavigationManager.NavigateTo($"search/results/{search}");
        }
    }
}
