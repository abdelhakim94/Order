using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Services;
using Order.Shared.Dto.Dish;

namespace Order.Client.Components
{
    public partial class DishDetailsModal : ComponentBase
    {
        private Spinner spinner;
        private Modal modal;
        private DishDetailsDto dish;
        private int IdDish { get; set; }
        private int IdSection { get; set; }
        private bool shouldReloadDish;

        private string pictureUrl { get => $"background-image:url({dish?.Picture})"; }

        private bool OptionsUnfolded = true;
        private bool ExtrasUnfolded = true;

        private HashSet<int> SelectedOptions = new();
        private HashSet<int> SelectedExtras = new();

        [Parameter]
        public Toast Toast { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback<SectionDishOptionsAndExtrasDto> OnChoose { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (shouldReloadDish)
            {
                await GetDishDetails();
                shouldReloadDish = false;
                StateHasChanged();
            }
        }

        async Task GetDishDetails()
        {
            spinner?.Show();
            dish = await HubConnection.Invoke<DishDetailsDto, int>("GetDishDetails", IdDish, Toast);
            spinner?.Hide();
        }

        public void Show(SectionDishOptionsAndExtrasDto dishInfos)
        {
            IdDish = dishInfos.IdDish;
            IdSection = dishInfos.IdSection;
            if (dishInfos.Options is not null) SelectedOptions.UnionWith(dishInfos.Options);
            if (dishInfos.Extras is not null) SelectedExtras.UnionWith(dishInfos.Extras);
            shouldReloadDish = true;
            modal.Show();
        }

        public async Task Close()
        {
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync();
            }
            await modal.Close();
            ResetState();
            StateHasChanged();
        }

        void OnSelectedOption(int id) => SelectedOptions.Add(id);
        void OnUnselectedOption(int id) => SelectedOptions.Remove(id);
        void OnSelectedExtra(int id) => SelectedExtras.Add(id);
        void OnUnselectedExtra(int id) => SelectedExtras.Remove(id);

        async Task Choose()
        {
            if (OnChoose.HasDelegate)
            {
                await OnChoose.InvokeAsync(new SectionDishOptionsAndExtrasDto
                {
                    IdDish = this.IdDish,
                    IdSection = this.IdSection,
                    Options = SelectedOptions,
                    Extras = SelectedExtras,
                });
            }
            await Close();
        }

        void ResetState()
        {
            dish = default(DishDetailsDto);
            IdDish = default(int);
            IdSection = default(int);
            OptionsUnfolded = true;
            ExtrasUnfolded = true;
            SelectedOptions = new();
            SelectedExtras = new();
            shouldReloadDish = true;
        }
    }

    public class SectionDishOptionsAndExtrasDto
    {
        public int IdDish { get; set; }
        public int IdSection { get; set; }
        public HashSet<int> Options { get; set; }
        public HashSet<int> Extras { get; set; }
    }
}
