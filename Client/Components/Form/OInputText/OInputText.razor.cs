using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components.Form
{
    public partial class OInputText
    {
        private string value;

        [Parameter]
        public string Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    if (ValueChanged.HasDelegate)
                        ValueChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private bool isHiddenValue;

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public bool IsSensitiveInput { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            this.isHiddenValue = parameters.GetValueOrDefault<bool>("IsSensitiveInput");
            return base.SetParametersAsync(parameters);
        }

        string ShouldHideValue() => isHiddenValue ? "password" : "";

        void ToggleIsHiddenValue() => isHiddenValue = !isHiddenValue;

        string GetIcon() => isHiddenValue ? "/icons/show-password.png" : "/icons/hide-password.png";
    }
}
