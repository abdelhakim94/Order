using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class Column : ComponentBase
    {
        string justifyStyle { get => $"justify-content: {Justify};"; }
        string alignStyle { get => $"align-items: {Align};"; }
        string wrapStyle { get => $"flex-wrap: {Wrap};"; }
        string expandHorizStyle { get => $"width: {toPercentage(ExpandHorizontally)};"; }
        string expandVertStyle { get => $"height: {toPercentage(ExpandVertically)};"; }

        string toPercentage(bool value) => value ? "100%" : "";

        [Parameter]
        public string Justify { get; set; }

        [Parameter]
        public string Align { get; set; }

        [Parameter]
        public string Wrap { get; set; }

        [Parameter]
        public bool ExpandHorizontally { get; set; }

        [Parameter]
        public bool ExpandVertically { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}