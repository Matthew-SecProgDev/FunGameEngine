using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fun.Engine.UI
{
    // TODO: it must be refactored and inherited from BaseUIObject
    public class DialogBox_Button : UIElement  // Simplified version for the example
    {
        private Action onClick;
        private string text;
        private SpriteFont font;

        public DialogBox_Button(Vector2 position, string text, SpriteFont font, Action onClickAction)
            : base(position)
        {
            this.onClick = onClickAction;
            this.text = text;
            this.font = font;
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && IsMouseOver())
            {
                onClick.Invoke();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)//Sprites sprites
        {
            // Draw button background and text
            spriteBatch.DrawString(font, text, Position, Color.White);
        }

        private bool IsMouseOver()
        {
            // Check if the mouse is over the button
            // This method needs proper implementation
            return true;  // Simplified for example purposes
        }
    }

    public class DialogBox : UIElement
    {
        private Texture2D backgroundTexture;
        private SpriteFont font;
        private string message;
        private Vector2 messagePosition;
        private Rectangle backgroundRectangle;
        private Action<bool> onResponse;  // Callback for handling responses

        private DialogBox_Button yesButton;
        private DialogBox_Button noButton;

        public DialogBox(Vector2 position, Texture2D background, SpriteFont font, string message, Action<bool> responseAction)
            : base(position)
        {
            this.backgroundTexture = background;
            this.font = font;
            this.message = message;
            this.onResponse = responseAction;

            this.backgroundRectangle = new Rectangle((int)position.X, (int)position.Y, 200, 100); // Assuming fixed size for simplicity
            this.messagePosition = new Vector2(position.X + 10, position.Y + 10);

            yesButton = new DialogBox_Button(new Vector2(position.X + 30, position.Y + 60), "Yes", font, () => onResponse(true));
            noButton = new DialogBox_Button(new Vector2(position.X + 110, position.Y + 60), "No", font, () => onResponse(false));
        }

        public override void Update(GameTime gameTime)
        {
            yesButton.Update(gameTime);
            noButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)//Sprites sprites
        {
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            spriteBatch.DrawString(font, message, messagePosition, Color.Black);
            yesButton.Draw(spriteBatch);
            noButton.Draw(spriteBatch);
        }
    }
}