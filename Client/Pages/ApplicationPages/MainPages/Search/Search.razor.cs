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
    public partial class Search : ComponentBase, IDisposable
    {
        private Spinner spinner;
        private bool canDispose;

        ValueWrapperDto<string> SearchValue { get; set; } = new(string.Empty);
        CloneableList<CategoryListItemDto> Categories = new();
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
        public MainLayout TopLayout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                canDispose = true;
                if (BottomLayout is not null) BottomLayout.SearchSelected = true;
                if (TopLayout is not null) TopLayout.PreviousPage = string.Empty;

                Categories = Store.Get<CloneableList<CategoryListItemDto>>(StoreKey.CATEGORIES);
                if (Categories is null)
                {
                    spinner?.Show();
                    Categories = await HubConnection.Invoke<CloneableList<CategoryListItemDto>>("GetCategories", Toast);
                    Categories = Categories is null ? new() : Categories;
                    Store.Set(StoreKey.CATEGORIES, Categories);
                    spinner?.Hide();
                }

                CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
                if (CurrentAddress is null)
                {
                    spinner?.Show();
                    CurrentAddress = await HubConnection.Invoke<UserAddressDetailDto>("GetLastUsedAddress", Toast);
                    spinner?.Hide();
                    if (CurrentAddress is not null)
                    {
                        Store.Set(StoreKey.ADDRESS, CurrentAddress);
                    }
                    else
                    {
                        AddressModal.Show();
                    }
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

        public void Dispose()
        {
            if (canDispose)
            {
                Store.OnUpdate -= OnStoreAddressChange;
                if (TopLayout is not null) TopLayout.PreviousPage = "search/";
            }
        }
    }
}
