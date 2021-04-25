using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto;
using Order.Shared.Dto.Address;

namespace Order.Client.Components
{
    public partial class AddressModal : ComponentBase, IDisposable
    {
        Modal Modal;
        UserAddressDetailDto CurrentAddress = new();
        List<DatalistOption> Options { get; set; } = new();
        Timer timer { get; set; }

        [Inject]
        public IStateStore Store { get; set; }

        [Inject]
        public IHubConnectionService HubConnection { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS) ?? CurrentAddress;
            Store.OnUpdate += OnStoreAddressChanged;
            timer = new Timer(400);
            timer.AutoReset = false;
            timer.Elapsed += async (s, e) => await OnTimedEvent(s, e);
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
            Options = await HubConnection.Invoke<List<DatalistOption>, string>("SearchCities", CurrentAddress.City);
            StateHasChanged();
        }

        private void ResetTimer()
        {
            timer.Stop();
            timer.Start();
        }

        void OnCityChanged(string value)
        {
            ResetTimer();
        }

        async Task HandleAddressSave(EditContext context)
        {
            Store.Set(StoreKey.ADDRESS, CurrentAddress);
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
