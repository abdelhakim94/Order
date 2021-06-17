using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class DishOrChefToggle : ComponentBase
    {
        [Parameter]
        public string LeftLabel { get; set; }

        [Parameter]
        public string RightLabel { get; set; }

        /// <summary>
        /// false means the left option is selected, which is the
        /// default switch position.
        /// </summary>
        /// <value></value>
        [Parameter]
        public bool Value { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        async Task OnSelect(ChangeEventArgs args)
        {
            if (args?.Value as string == "left")
                Value = false;
            else if (args?.Value as string == "right")
                Value = true;
            else return;

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
