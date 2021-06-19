using Microsoft.AspNetCore.Components;
using Order.Client.Constants;

namespace Order.Client.Layouts
{
    public partial class MainPagesLayout : LayoutComponentBase
    {
        // This is really bad design choice. Should refactor. Violation of the Open-Closed principle.
        // This implies that if a new page is added, the same pattern below
        // will be applied to it. This layout should instead have a method (AddPage)
        // that adds the required behaviour to the page.
        private int translateSearchIcon { get => searchSelected ? 0 : 100; }
        private bool searchSelected;
        public bool SearchSelected
        {
            get => searchSelected;
            set
            {
                if (searchSelected == value) return;
                searchSelected = value;
                if (value)
                {
                    IWantSelected = false;
                    OrdersSelected = false;
                    NavigationManager.NavigateTo("search/");
                }
            }
        }

        private int translateIwantIcon { get => iWantSelected ? 0 : 100; }
        private bool iWantSelected;
        public bool IWantSelected
        {
            get => iWantSelected;
            set
            {
                if (iWantSelected == value) return;
                iWantSelected = value;
                if (value)
                {
                    SearchSelected = false;
                    OrdersSelected = false;
                    NavigationManager.NavigateTo("iwant/");
                }
            }
        }

        private int translateOrdersIcon { get => ordersSelected ? 0 : 100; }
        private bool ordersSelected;
        public bool OrdersSelected
        {
            get => ordersSelected;
            set
            {
                if (ordersSelected == value) return;
                ordersSelected = value;
                if (value)
                {
                    SearchSelected = false;
                    IWantSelected = false;
                    NavigationManager.NavigateTo("orders/");
                }
            }
        }
    }
}
