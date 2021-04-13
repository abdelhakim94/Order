using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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

        [Parameter]
        public bool HideData { get; set; }

        [Parameter]
        public string RightIcon { get; set; }

        [Parameter]
        public EventCallback OnRightIconClick { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        private string ShouldHideValue() => HideData ? "password" : "";

        async Task HandleInputChange(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
        }

        async Task HandleRightIconClick(MouseEventArgs args)
        {
            if (OnRightIconClick.HasDelegate)
            {
                await OnRightIconClick.InvokeAsync();
            }
        }
    }
}
