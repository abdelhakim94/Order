using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class OCheckBox : ComponentBase
    {
        private bool value;

        [Parameter]
        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                if (ValueChanged.HasDelegate)
                    ValueChanged.InvokeAsync(this.value);
            }
        }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
