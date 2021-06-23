using System.Linq;
using Microsoft.AspNetCore.Components;
using Order.Client.Constants;
using Order.Client.Services;

namespace Order.Client.Layouts
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IHistoryService HistoryService { get; set; }

        private RenderFragment Top;
        public void SetTop(RenderFragment top)
        {
            Top = top;
            base.StateHasChanged();
        }

        public bool DisplayPreviousPage { get; set; } = true;
        private string justifyTopContent
        {
            get => Top is null && !DisplayPreviousPage
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
