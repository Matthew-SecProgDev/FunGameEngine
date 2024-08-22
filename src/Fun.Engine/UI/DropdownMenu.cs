using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.UI
{
    // TODO: it must be refactored and inherited from BaseUIObject
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
            this.selectedIndex = 0;  // default to the first option
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
                    // Toggle the expansion of the dropdown
                    isExpanded = !isExpanded;
                }
                else if (isExpanded && menuBounds.Contains(mouseState.Position))
                {
                    // Calculate which option was selected
                    int optionIndex = (mouseState.Y - menuBounds.Y) / font.LineSpacing;
                    if (optionIndex < options.Count)
                    {
                        selectedIndex = optionIndex;
                        isExpanded = false;  // Collapse the menu after selection
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)//Sprites sprites
        {
            spriteBatch.Draw(backgroundTexture, selectionBounds, Color.White);  // Draw the selected item area
            spriteBatch.DrawString(font, SelectedOption, new Vector2(selectionBounds.X + 5, selectionBounds.Y + 5), Color.Black);

            if (isExpanded)
            {
                // Draw all options
                for (int i = 0; i < options.Count; i++)
                {
                    var optionPosition = new Vector2(menuBounds.X + 5, menuBounds.Y + (i * font.LineSpacing) + 5);
                    spriteBatch.DrawString(font, options[i], optionPosition, Color.Black);
                }
            }
        }
    }
}