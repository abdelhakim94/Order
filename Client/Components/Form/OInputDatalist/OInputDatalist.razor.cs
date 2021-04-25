using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Shared.Dto;

namespace Order.Client.Components.Form
{
    public partial class OInputDatalist
    {
        [Parameter]
        public IEnumerable<DatalistOption> Options { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        // ValueChanged is for two way binding. This one is for custom behavior.
        [Parameter]
        public EventCallback<string> OnValueChanged { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, Object> AdditionalAttributes { get; set; }

        async Task HandleInputChange(ChangeEventArgs args)
        {
            Value = args.Value.ToString();
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
            if (OnValueChanged.HasDelegate)
                await OnValueChanged.InvokeAsync(Value);
        }
    }
}
