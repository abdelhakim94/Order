using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Spinner : ComponentBase
    {
        private bool show { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        public void Show()
        {
            show = true;
            if (OnChange.HasDelegate)
                OnChange.InvokeAsync(show);
            StateHasChanged();
        }

        public void Hide()
        {
            show = false;
            if (OnChange.HasDelegate)
                OnChange.InvokeAsync(show);
            StateHasChanged();
        }
    }
}