using System;
using Fun.Engine.UI.Buttons;
using Fun.Engine.UI.Buttons.Models;
using Fun.Engine.UI.Labels;
using Fun.Engine.UI.Labels.Models;
using Fun.Engine.UI.Sliders;
using Fun.Engine.UI.Sliders.Models;
using Fun.Engine.UI.TextFields;
using Fun.Engine.UI.TextFields.Models;
using Fun.Engine.UI.ToggleSwitchs;
using Fun.Engine.UI.ToggleSwitchs.Models;
using Fun.Engine.UI.Tooltips;
using Fun.Engine.UI.Tooltips.Models;
using Fun.SimpleGame.UI.Sliders;
using Fun.SimpleGame.UI.Sliders.Models;
using Fun.SimpleGame.UI.Tooltips;
using Fun.SimpleGame.UI.Tooltips.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fun.SimpleGame.UI
{
    public static class UIExtensions
    {
        public static Button CreateButton(
            Vector2 size,
            Texture2D texture,
            string text,
            Color penColor,
            SpriteFont font,
            Action onClick,
            Vector2 position)
        {
            var option = new TextButtonOption
            {
                Text = text,
                PenColor = penColor,
                Font = font,
                NormalTexture = texture,
                OnClick = onClick
            };

            return new Button(size, option, position);
        }

        public static Button CreateButton(
            Vector2 size,
            Texture2D normalTexture,
            Texture2D hoverTexture,
            Texture2D pressedTexture,
            Action onClick,
            Vector2 position)
        {
            var option = new TexturedButtonOption
            {
                NormalTexture = normalTexture,
                HoverTexture = hoverTexture,
                PressedTexture = pressedTexture,
                OnClick = onClick
            };

            return new Button(size, option, position);
        }

        public static TextField CreateTextField(
            Vector2 size,
            Texture2D texture,
            Color penColor,
            SpriteFont font,
            Vector2 position)
        {
            var option = new TextFieldOption
            {
                Texture = texture,
                PenColor = penColor,
                Font = font
            };

            return new TextField(size, option, position);
        }

        public static Label CreateLabel(
            Vector2 size,
            string text,
            Color penColor,
            SpriteFont font,
            Vector2 position)
        {
            var option = new LabelOption
            {
                Text = text,
                PenColor = penColor,
                Font = font
            };

            return new Label(size, option, position);
        }

        public static Slider CreateSlider(
            Vector2 size,
            Texture2D backgroundTexture,
            Texture2D handleTexture,
            Vector2 handleSize,
            Texture2D fillTexture,
            float fillHeight,
            float initialValue,
            float minValue,
            float maxValue,
            Vector2 position)
        {
            var option = new SliderOption
            {
                BackgroundTexture = backgroundTexture,
                HandleTexture = handleTexture,
                HandleSize = handleSize,
                FillTexture = fillTexture,
                FillHeight = fillHeight,
                InitialValue = initialValue,
                MinValue = minValue,
                MaxValue = maxValue
            };

            return new Slider(size, option, position);
        }

        public static ToggleSwitch CreateToggleSwitch(
            Vector2 size,
            Texture2D onTexture,
            Texture2D offTexture,
            DefaultState defaultState,
            Vector2 position)
        {
            var option = new ToggleSwitchOption
            {
                OnTexture = onTexture,
                OffTexture = offTexture,
                State = defaultState
            };

            return new ToggleSwitch(size, option, position);
        }

        public static CloseableTooltip CreateCloseableTooltip(
            Vector2 size,
            Texture2D backgroundTexture,
            Vector2 buttonSize,
            Texture2D buttonTexture,
            Vector2 buttonPosition,
            Color penColor,
            SpriteFont font,
            Vector2 position)
        {
            var option = new CloseableTooltipOption
            {
                BackgroundTexture = backgroundTexture,
                ButtonTexture = buttonTexture,
                ButtonSize = buttonSize,
                ButtonPosition = buttonPosition,
                PenColor = penColor,
                Font = font
            };

            return new CloseableTooltip(size, option, position);
        }

        public static SimpleTooltip CreateSimpleTooltip(
            Vector2 size,
            Texture2D backgroundTexture,
            string text,
            Color penColor,
            SpriteFont font,
            Vector2 position)
        {
            var option = new TooltipOption
            {
                BackgroundTexture = backgroundTexture,
                Text = text,
                PenColor = penColor,
                Font = font
            };

            return new SimpleTooltip(size, option, position);
        }

        public static SliderWithTooltip CreateSliderWithTooltip(
            Vector2 size,
            Texture2D backgroundTexture,
            Texture2D handleTexture,
            Vector2 handleSize,
            Texture2D fillTexture,
            float fillHeight,
            float initialValue,
            float minValue,
            float maxValue,
            BaseTooltip tooltip,
            Vector2 position)
        {
            var option = new SliderWithTooltipOption
            {
                BackgroundTexture = backgroundTexture,
                HandleTexture = handleTexture,
                HandleSize = handleSize,
                FillTexture = fillTexture,
                FillHeight = fillHeight,
                InitialValue = initialValue,
                MinValue = minValue,
                MaxValue = maxValue,
                Tooltip = tooltip
            };

            return new SliderWithTooltip(size, option, position);
        }
    }
}