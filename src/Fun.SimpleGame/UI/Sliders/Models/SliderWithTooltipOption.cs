using Fun.Engine.UI.Sliders.Models;
using Fun.Engine.UI.Tooltips;

namespace Fun.SimpleGame.UI.Sliders.Models
{
    public class SliderWithTooltipOption : SliderOption
    {
        public required BaseTooltip Tooltip { get; init; }
    }
}