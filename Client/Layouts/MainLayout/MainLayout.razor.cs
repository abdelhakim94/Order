using Microsoft.AspNetCore.Components;
using Order.Client.Constants;

namespace Order.Client.Layouts
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private RenderFragment Top;
        public void SetTop(RenderFragment top)
        {
            Top = top;
            base.StateHasChanged();
        }

        public string PreviousPage { get; set; }
        private string justifyTopContent
        {
            get => Top is null && string.IsNullOrWhiteSpace(PreviousPage)
            ? JustifyContent.FLEX_END
            : JustifyContent.SPACE_BETWEEN;
        }

        private int translateCartIcon { get => cartSelected ? 0 : 100; }
        private bool cartSelected;
        public bool CartSelected
        {
            get => cartSelected;
            set
            {
                if (cartSelected == value) return;
                cartSelected = value;
                if (value)
                {
                    NavigationManager.NavigateTo("cart/");
                }
                StateHasChanged();
            }
        }

        public int NbElements { get; set; } = 1;
    }
}