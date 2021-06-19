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
    public partial class MenuDetails : ComponentBase, IDisposable
    {
        private bool canDispose;

        private MenuDetailsDto menu { get; set; }
        private int quantity { get; set; } = 1;
        private DishDetailsModal dishDetailsModal;

        private bool optionsUnfolded = true;
        private bool extrasUnfolded = true;
        private Dictionary<int, bool> sectionsUnfolded = new();

        private HashSet<int> SelectedOptions = new();
        private HashSet<int> SelectedExtras = new();
        //All three dictionaries have IdSection as key.
        // "selectedSectionDish" maps the section to the chosen dish in that section
        // "sectionDishOptions" maps the section to the chosen options of the chosen dish in that section
        // "sectionDishExtras" maps the section to the chosen extras of the chosen dish in that section
        private Dictionary<int, int?> selectedSectionDish = new();
        private Dictionary<int, HashSet<int>> sectionDishOptions = new();
        private Dictionary<int, HashSet<int>> sectionDishExtras = new();

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

        void OnSelectSectionDish(SectionDishOptionsAndExtrasDto dishInfos)
        {
            if (selectedSectionDish?.ContainsKey(dishInfos.IdSection) is true
                && sectionDishOptions.ContainsKey(dishInfos.IdSection) is true
                && sectionDishExtras.ContainsKey(dishInfos.IdSection) is true)
            {
                selectedSectionDish[dishInfos.IdSection] = dishInfos.IdDish;
                sectionDishOptions[dishInfos.IdSection] = dishInfos.Options;
                sectionDishExtras[dishInfos.IdSection] = dishInfos.Extras;
            }
        }

        void OnChefDataClick()
        {
            if (!string.IsNullOrWhiteSpace(Search))
            {
                NavigationManager.NavigateTo($"ChefDetails/{Search}/{menu?.ChefId}");
            }
            else
            {
                NavigationManager.NavigateTo($"ChefDetails/{menu?.ChefId}");
            }
        }

        async Task ShowDishDetailsModal(int idDish, int idSection)
        {
            await dishDetailsModal?.Show(new SectionDishOptionsAndExtrasDto
            {
                IdDish = idDish,
                IdSection = idSection,
                Options = selectedSectionDish?.ContainsKey(idSection) is true && selectedSectionDish[idSection] == idDish
                    ? sectionDishOptions[idSection]
                    : null,
                Extras = selectedSectionDish?.ContainsKey(idSection) is true && selectedSectionDish[idSection] == idDish
                    ? sectionDishExtras[idSection]
                    : null,
            });
        }

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
