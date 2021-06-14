using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Order.Client.Components
{
    public partial class SocialMediaButton : ComponentBase
    {
        [Parameter]
        public string Type { get; set; } = "button";

        /// <summary>
        /// Path to the social media backgroun image.
        /// </summary>
        [Parameter]
        public string SocialMediaPath { get; set; }

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

        [Parameter]
        public string CssClass { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AdditionalAttributes { get; set; }
    }
}
