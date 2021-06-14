using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class SelectorOption<T> : ComponentBase
    {
        [Parameter]
        public T Id { get; set; }

        private bool selected;
        [Parameter]
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (SelectedChanged.HasDelegate)
                {
                    SelectedChanged.InvokeAsync(value);
                }
                if (value && OnSelect.HasDelegate)
                {
                    OnSelect.InvokeAsync(Id);
                }
                else if (!value && OnUnselect.HasDelegate)
                {
                    OnUnselect.InvokeAsync(Id);
                }
            }
        }

        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<T> OnSelect { get; set; }

        [Parameter]
        public EventCallback<T> OnUnselect { get; set; }
    }
}
