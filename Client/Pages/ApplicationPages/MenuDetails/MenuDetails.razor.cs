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
    public partial class MenuDetails : ComponentBase
    {
        Spinner spinner;

        MenuDetailsDto menu { get; set; }
        int quantity { get; set; } = 1;
        DishDetailsModal dishDetailsModal;

        bool optionsUnfolded = true;
        bool extrasUnfolded = true;
        Dictionary<int, bool> sectionsUnfolded = new();

        HashSet<int> SelectedOptions = new();
        HashSet<int> SelectedExtras = new();

        //All three dictionaries have IdSection as key.
        // "selectedSectionDish" maps the section to the chosen dish in that section
        // "sectionDishOptions" maps the section to the chosen options of the chosen dish in that section
        // "sectionDishExtras" maps the section to the chosen extras of the chosen dish in that section
        Dictionary<int, int?> selectedSectionDish = new();
        Dictionary<int, HashSet<int>> sectionDishOptions = new();
        Dictionary<int, HashSet<int>> sectionDishExtras = new();

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Search { get; set; }

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
            if (firstRender)
            {
                await GetMenuDetails();
                StateHasChanged();
            }
        }

        async Task GetMenuDetails()
        {
            spinner?.Show();
            menu = await HubConnection.Invoke<MenuDetailsDto, int>("GetMenuDetails", Id, Toast);
            spinner?.Hide();
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

        void ShowDishDetailsModal(int idDish, int idSection)
        {
            dishDetailsModal?.Show(new SectionDishOptionsAndExtrasDto
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
    }
}
