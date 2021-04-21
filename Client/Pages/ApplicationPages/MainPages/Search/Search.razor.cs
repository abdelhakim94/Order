using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Shared.Dto.Address;
using Order.Shared.Dto.Category;

namespace Order.Client.Pages
{
    public partial class Search : ComponentBase, IDisposable
    {
        List<CategoryListItemDto> Categories = new();
        CategorySearchBarDto SearchValue { get; set; } = new();
        UserAddressDetailDto CurrentAddress { get; set; } = new();
        List<UserAddressDetailDto> AllAddresses { get; set; } = new();

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public IStateStore Store { get; set; }

        [CascadingParameter]
        public MainPagesLayout Layout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.SearchSelected = true;
            Store.OnUpdate += OnAddressChange;

            try
            {
                Categories = Store.Get<List<CategoryListItemDto>>(StoreKey.CATEGORIES);
                if (Categories is null)
                {
                    Categories = await HubConnection.Invoke<List<CategoryListItemDto>>("GetCategories");
                    Store.Set(StoreKey.CATEGORIES, Categories);
                }

                CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
                if (CurrentAddress is null)
                {
                    CurrentAddress = await HubConnection.Invoke<UserAddressDetailDto>("GetLastUsedAddress");
                    Store.Set(StoreKey.ADDRESS, CurrentAddress);
                }
            }
            catch (System.Exception ex) when (ex is ApplicationException)
            {
                Toast.ShowError(ex.Message);
            }
            catch (System.Exception)
            {
                Toast.ShowError(UIMessages.DefaultInternalError);
            }
        }

        string GetFullAddress()
        {
            if (CurrentAddress.Address1 is not null)
            {
                return $"{CurrentAddress.Address1}, {CurrentAddress.Address2}, {CurrentAddress.ZipCode} {CurrentAddress.City}";
            }
            return string.Empty;
        }

        void OnAddressChange(object sender, StoreUpdateArgs args)
        {
            if (args.Key is StoreKey.ADDRESS)
            {
                CurrentAddress = args.Value as UserAddressDetailDto;
            }
        }

        public void Dispose()
        {
            Store.OnUpdate -= OnAddressChange;
        }
    }
}
