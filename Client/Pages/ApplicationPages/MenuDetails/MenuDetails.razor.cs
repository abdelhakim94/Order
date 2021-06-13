using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Components.Misc;
using Order.Client.Layouts;
using Order.Client.Services;
using Order.Shared.Dto.Dish;

namespace Order.Client.Pages
{
    public partial class MenuDetails : ComponentBase, IDisposable
    {
        private bool canDispose;

        private MenuDetailsDto menu { get; set; }
        private int quantity { get; set; } = 1;

        private bool optionsUnfolded = true;
        private bool extrasUnfolded = true;
        private Dictionary<int, bool> sectionsUnfolded = new();

        private List<int> SelectedOptions = new();
        private List<int> SelectedExtras = new();
        private Dictionary<int, int?> selectedSectionDish = new();

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Search { get; set; }

        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            canDispose = false;
            await GetMenuDetails();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            // Entering the details page should be done from a list page.
            if (!string.IsNullOrWhiteSpace(Search))
            {
                if (MainLayout is not null)
                {
                    MainLayout.PreviousPage = $"search/results/{Search}";
                }
            }
            else
            {
                MainLayout.PreviousPage = "search/";
                return;
            }
            canDispose = true;
        }

        async Task GetMenuDetails()
        {
            Spinner.Show();
            menu = await HubConnection.Invoke<MenuDetailsDto, int>("GetMenuDetails", Id, Toast);
            Spinner.Hide();
        }

        string getBackgroundPicture(string url) => $"background-image:url({url})";

        void OnSelectedOption(int id) => SelectedOptions.Add(id);
        void OnUnselectedOption(int id) => SelectedOptions.Remove(id);

        void OnSelectedExtra(int id) => SelectedExtras.Add(id);
        void OnUnselectedExtra(int id) => SelectedExtras.Remove(id);

        public void Dispose()
        {
            if (canDispose)
            {
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    MainLayout.PreviousPage = $"MenuDetails/{Search}/{Id}";
                }
                else
                {
                    MainLayout.PreviousPage = $"MenuDetails/{Search}/{Id}";
                }
            }
        }
    }
}
