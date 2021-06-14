using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Client.Constants;

namespace Order.Client
{
    public partial class App : ComponentBase
    {
        public Toast Toast { get; set; }
        public Spinner Spinner { get; set; }
        public bool Blure { get; set; }
        private string classBlured { get => Blure ? $"app-container {CSSCLasses.PageBlured}" : "app-container"; }
    }
}
