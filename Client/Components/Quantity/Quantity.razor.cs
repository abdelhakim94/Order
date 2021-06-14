using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Quantity : ComponentBase
    {
        private string disableDecrement { get => Value == 1 ? "color: lightslategray;" : string.Empty; }
        [Parameter]
        public int Value { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        async Task OnIncrement()
        {
            Value++;
            if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(Value);
        }

        async Task OnDecrement()
        {
            if (Value == 1) return;
            Value--;
            if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(Value);
        }
    }
}
