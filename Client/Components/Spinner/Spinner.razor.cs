using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Spinner : ComponentBase
    {
        private bool show { get; set; }
        private string isVisible { get => show ? "visible" : "invisible"; }
        private string isDisabled { get => show ? "disabled" : string.Empty; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

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
