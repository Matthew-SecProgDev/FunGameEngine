using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.UI
{
    public class DropdownMenu : UIElement
    {
        private List<string> options;
        private bool isExpanded;
        private int selectedIndex;
        private SpriteFont font;
        private Texture2D backgroundTexture;
        private Rectangle menuBounds;
        private Rectangle selectionBounds;

        public string SelectedOption => options[selectedIndex];

        public DropdownMenu(Vector2 position, SpriteFont font, Texture2D background, List<string> options)
            : base(position)
        {
            this.font = font;
            this.backgroundTexture = background;
            this.options = new List<string>(options);
            this.selectedIndex = 0;
            this.isExpanded = false;
            this.menuBounds = new Rectangle((int)position.X, (int)position.Y, background.Width, background.Height);
            this.selectionBounds = new Rectangle((int)position.X, (int)position.Y, background.Width, background.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsVisible) return;

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (selectionBounds.Contains(mouseState.Position))
                {
                    isExpanded = !isExpanded;
                }
                else if (isExpanded && menuBounds.Contains(mouseState.Position))
                {
                    int optionIndex = (mouseState.Y - menuBounds.Y) / font.LineSpacing;
                    if (optionIndex < options.Count)
                    {
                        selectedIndex = optionIndex;
                        isExpanded = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, selectionBounds, Color.White);
            spriteBatch.DrawString(font, SelectedOption, new Vector2(selectionBounds.X + 5, selectionBounds.Y + 5), Color.Black);

            if (isExpanded)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    var optionPosition = new Vector2(menuBounds.X + 5, menuBounds.Y + (i * font.LineSpacing) + 5);
                    spriteBatch.DrawString(font, options[i], optionPosition, Color.Black);
                }
            }
        }
    }
}