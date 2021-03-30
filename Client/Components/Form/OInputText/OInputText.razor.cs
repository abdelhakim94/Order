using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components.Form
{
    public partial class OInputText : ComponentBase
    {
        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        private bool isHiddenValue;

        [Parameter]
        public bool IsSensitiveInput { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        protected override void OnInitialized()
        {
            this.isHiddenValue = IsSensitiveInput;
        }

        string ShouldHideValue() => isHiddenValue ? "password" : "";

        void ToggleIsHiddenValue() => isHiddenValue = !isHiddenValue;

        string GetIcon() => isHiddenValue ? "icons/show-password.png" : "icons/hide-password.png";

        async Task HandleInputChange(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
        }
    }
}
