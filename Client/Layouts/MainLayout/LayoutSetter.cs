using Microsoft.AspNetCore.Components;

namespace Order.Client.Layouts
{
    public class LayoutSetter : ComponentBase
    {
        [CascadingParameter]
        public MainLayout MainLayout { get; set; }

        [Parameter]
        public RenderFragment Top { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            MainLayout.SetTop(Top);
        }
    }
}
