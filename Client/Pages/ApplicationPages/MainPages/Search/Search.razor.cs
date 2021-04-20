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
    public partial class Search : ComponentBase
    {
        private List<CategoryListItemDto> categories = new();
        public CategorySearchBarDto SearchValue { get; set; } = new();
        public UserAddressDetailDto CurrentAddress { get; set; } = new();
        public List<UserAddressDetailDto> AllAddresses { get; set; } = new();

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [CascadingParameter]
        public MainPagesLayout Layout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.SearchSelected = true;
            try
            {
                var catTask = HubConnection.Invoke<List<CategoryListItemDto>>("GetCategories");
                var curAddTask = HubConnection.Invoke<UserAddressDetailDto>("GetLastUsedAddress");
                categories = await catTask;
                CurrentAddress = await curAddTask;
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
            return $"{CurrentAddress.Address1}, {CurrentAddress.Address2}, {CurrentAddress.ZipCode} {CurrentAddress.City}";
        }
    }
}
