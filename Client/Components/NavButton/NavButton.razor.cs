using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class NavButton : ComponentBase
    {
        /// <summary>
        /// True for displaying as a row, false for displaying as column.
        /// </summary>
        [Parameter]
        public bool DisplayHorizontal { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Translates the background image horizontally.
        /// A percentage value.
        /// Useful when using sprites.
        /// </summary>
        [Parameter]
        public double Translate { get; set; }

        /// <summary>
        /// The number of images in the sprite.
        /// Images in a sprite should be distributed
        /// evenly.
        /// </summary>
        [Parameter]
        public int ImagesInSprite { get; set; }

        private string labelColor { get => Selected ? "black" : "lightgrey"; }
    }
}
