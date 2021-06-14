using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;

namespace Order.Client.Components
{
    public partial class AddressModal : ComponentBase, IDisposable
    {
        Modal Modal;
        Timer timer { get; set; }
        UserAddressDetailDto CurrentAddress = new();
        List<IdentifiedUserAddressDetailDto> AllAddresses = new();
        string SelectedRecentAddress
        {
            get =>
                AllAddresses?.FirstOrDefault(a => a.Id == CurrentAddress?.ToString())?.Id;
            set =>
                CurrentAddress = AllAddresses?.FirstOrDefault(a => a.Id == value).Address.Clone();
        }

        List<DatalistOption> CityOptions { get; set; } = new();
        IEnumerable<DatalistOption> RecentAddressOptions
        {
            get => AllAddresses.Select(a => new DatalistOption
            {
                Id = a.Id,
                Value = a.Address.ToString(),
            });
        }

        [Inject]
        public IStateStore Store { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [CascadingParameter]
        public Toast Toast { get; set; }

        [CascadingParameter]
        public Spinner Spinner { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Store.OnUpdate += OnStoreAddressChanged;
            timer = new Timer(400);
            timer.AutoReset = false;
            timer.Elapsed += async (s, e) => await OnTimedEvent(s, e);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS) ?? CurrentAddress;
            AllAddresses = await HubConnection.Invoke<List<IdentifiedUserAddressDetailDto>>("GetAllUserAddresses", Toast);
            AllAddresses = AllAddresses is null ? new() : AllAddresses;
        }

        public void Show()
        {
            Modal.Show();
        }

        public async Task Close()
        {
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync();
            }
            await Modal.Close();
        }

        async Task OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            CityOptions = await HubConnection.Invoke<List<DatalistOption>, string>("SearchCities", CurrentAddress.City, Toast);
            StateHasChanged();
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        void OnOptionChanged(DatalistOption option)
        {
            if (int.TryParse(option.Id, out var parsed))
                CurrentAddress.IdCity = parsed;
            ResetTimer();
        }

        async Task HandleAddressSave(EditContext context)
        {
            Spinner.Show();
            var result = await HubConnection.Invoke<bool, UserAddressDetailDto>("SaveUserAddress", CurrentAddress, Toast);
            if (result)
            {
                Store.Set(StoreKey.ADDRESS, CurrentAddress);
            }
            Spinner.Hide();
            await Close();
        }

        void OnStoreAddressChanged(object sender, StoreUpdateArgs args)
        {
            if (args.Key is StoreKey.ADDRESS)
            {
                CurrentAddress = args.Value as UserAddressDetailDto;
            }
        }

        public void Dispose()
        {
            Store.OnUpdate -= OnStoreAddressChanged;
            timer.Dispose();
        }
    }
}
