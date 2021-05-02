using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Components.Misc;
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
        CloneableList<CategoryListItemDto> Categories = new();
        CategorySearchBarDto SearchValue { get; set; } = new();
        UserAddressDetailDto CurrentAddress { get; set; } = new();
        AddressModal AddressModal;
        IEnumerable<DatalistOption> options { get; set; } = new List<DatalistOption> { };

        private bool isEditingAddress { get; set; }
        private string blured { get => isEditingAddress ? CSSCLasses.PageBlured : string.Empty; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public IStateStore Store { get; set; }

        [CascadingParameter]
        public MainPagesLayout BottomLayout { get; set; }

        [CascadingParameter]
        public MainLayout TopLayout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BottomLayout.SearchSelected = true;
            try
            {
                Categories = Store.Get<CloneableList<CategoryListItemDto>>(StoreKey.CATEGORIES);
                if (Categories is null)
                {
                    Categories = await HubConnection.Invoke<CloneableList<CategoryListItemDto>>("GetCategories");
                    Categories = Categories is null ? new() : Categories;
                    Store.Set(StoreKey.CATEGORIES, Categories);
                }

                CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS);
                if (CurrentAddress is null)
                {
                    CurrentAddress = await HubConnection.Invoke<UserAddressDetailDto>("GetLastUsedAddress");
                    CurrentAddress = CurrentAddress is null ? new() : CurrentAddress;
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
            finally
            {
                Store.OnUpdate += OnStoreAddressChange;
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

        void HandleAddressBarClick()
        {
            isEditingAddress = true;
            BottomLayout.Blured = true;
            TopLayout.Blured = true;
            AddressModal.Show();
        }

        void OnAddressModalClosed()
        {
            isEditingAddress = false;
            BottomLayout.Blured = false;
            TopLayout.Blured = false;
        }

        void OnStoreAddressChange(object sender, StoreUpdateArgs args)
        {
            if (args.Key is StoreKey.ADDRESS)
            {
                CurrentAddress = args.Value as UserAddressDetailDto;
                try
                {
                    HubConnection.Invoke<bool, UserAddressDetailDto>("SaveUserAddress", CurrentAddress);
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
        }

        public void Dispose()
        {
            Store.OnUpdate -= OnStoreAddressChange;
        }
    }
}
