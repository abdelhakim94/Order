using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Services;
using Order.Shared.Dto.Dish;

namespace Order.Client.Components
{
    public partial class DishDetailsModal : ComponentBase
    {
        private Modal modal;
        private DishDetailsDto dish;
        private int IdDish { get; set; }
        private int IdSection { get; set; }

        private string pictureUrl { get => $"background-image:url({dish?.Picture})"; }

        private bool OptionsUnfolded = true;
        private bool ExtrasUnfolded = true;

        private HashSet<int> SelectedOptions = new();
        private HashSet<int> SelectedExtras = new();

        [Parameter]
        public Spinner Spinner { get; set; }

        [Parameter]
        public Toast Toast { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback<SectionDishOptionsAndExtrasDto> OnChoose { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        async Task GetDishDetails()
        {
            Spinner.Show();
            dish = await HubConnection.Invoke<DishDetailsDto, int>("GetDishDetails", IdDish, Toast);
            Spinner.Hide();
        }

        public async Task Show(SectionDishOptionsAndExtrasDto dishInfos)
        {
            IdDish = dishInfos.IdDish;
            IdSection = dishInfos.IdSection;
            if (dishInfos.Options is not null) SelectedOptions.UnionWith(dishInfos.Options);
            if (dishInfos.Extras is not null) SelectedExtras.UnionWith(dishInfos.Extras);
            modal.Show();
            await GetDishDetails();
            StateHasChanged();
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
