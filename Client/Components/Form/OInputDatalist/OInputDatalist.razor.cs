using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
        public EventCallback<DatalistOption> OptionChanged { get; set; }

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
            {
                await ValueChanged.InvokeAsync(Value);
            }
            if (OptionChanged.HasDelegate)
            {
                var id = Options?.FirstOrDefault(o => o.Value.ToLower() == Value.ToLower())?.Id;
                await OptionChanged.InvokeAsync(new DatalistOption { Id = id, Value = Value });
            }
        }
    }
}
