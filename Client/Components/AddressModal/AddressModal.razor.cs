using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components.Form;
using Order.Client.Components.Misc;
using Order.Client.Constants;
using Order.Client.Services;
using Order.Shared.Dto.Address;

namespace Order.Client.Components
{
    public partial class AddressModal : ComponentBase, IDisposable
    {
        UserAddressDetailDto CurrentAddress = new();
        Modal Modal;

        [Inject]
        public IStateStore Store { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public IEnumerable<DatalistOption> Options { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            CurrentAddress = Store.Get<UserAddressDetailDto>(StoreKey.ADDRESS) ?? CurrentAddress;
            Store.OnUpdate += OnStoreAddressChanged;
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

        async Task HandleAddressSave()
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
        }
    }
}
