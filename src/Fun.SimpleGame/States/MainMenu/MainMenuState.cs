using System.Collections.Generic;
using Fun.Engine.Events;
using Fun.Engine.States;
using Fun.Engine.UI;
using Fun.Engine.UI.Labels;
using Fun.Engine.UI.Sliders;
using Fun.Engine.UI.ToggleSwitchs.Models;
using Microsoft.Xna.Framework;
using Ex = Fun.SimpleGame.UI.UIExtensions;

namespace Fun.SimpleGame.States.MainMenu
{
    // TODO: it's not completed, I have to refactor it

    enum MainMenuScreenState
    {
        Main = 0,

        StageSelection = 1,

        SettingOnline = 2,

        Option = 3
    }

    public class MainMenuState : BaseGameState
    {
        // if I have a handler for each component and use this for communicate with other components,
        // for example without create a new component and connect it to another
        private List<BaseUIObject> _elements = null!;

        //private MainMenuScreenState _mainMenuScreenState = MainMenuScreenState.Main;

        public MainMenuState() : base(new MainMenuInputMapper())
        {
        }

        public override void LoadContent()
        {
            var font = ResourceManager.LoadFont("Fonts/Font");

            var texture = ResourceManager.LoadTexture("UIElements/Button");
            var newGameTexture = ResourceManager.LoadTexture("UIElements/NewGame");
            var exitTexture = ResourceManager.LoadTexture("UIElements/Exit");
            //var chooseLevelTexture = ResourceManager.LoadTexture("UIElements/ChooseLevel");
            var optionsTexture = ResourceManager.LoadTexture("UIElements/Options");

            var trackTexture = ResourceManager.LoadTexture("UIElements/Slider/Track");
            var handleTexture = ResourceManager.LoadTexture("UIElements/Slider/Handle");
            var fillTexture = ResourceManager.LoadTexture("UIElements/Slider/Fill");
            var handleSize = new Vector2(25, 25);

            var onSwitchTexture = ResourceManager.LoadTexture("UIElements/ToggleSwitch/OnSwitch");
            var offSwitchTexture = ResourceManager.LoadTexture("UIElements/ToggleSwitch/OffSwitch");

            var buttonCloseTexture = ResourceManager.LoadTexture("UIElements/CloseButton");

            const int width = 360;
            const int height = 115;
            var penColor = Color.Black;
            var size = new Vector2(width, height);

            var simpleTooltip = Ex.CreateSimpleTooltip(new Vector2(70, 70), texture, "For Test", Color.Black, font,
                new Vector2(400, 185));

            var closeableTooltip = Ex.CreateCloseableTooltip(new Vector2(70, 70), texture, new Vector2(20, 20),
                buttonCloseTexture, new Vector2(12, 12), Color.Black, font, new Vector2(400, 100));

            _elements = new List<BaseUIObject>
            {
                //Ex.CreateButton(size, optionsTexture, null, penColor, font, Options_Clicked, new Vector2(400, 220)),
                //Ex.CreateButton(size, optionsTexture, "Join Online Game", penColor, font, JoinOnline_Clicked, new Vector2(400, 300)),

                //Ex.CreateToggleSwitch(new Vector2(150, 70), onSwitchTexture, offSwitchTexture, DefaultState.OffSwitch,
                //    new Vector2(400, 100)),

                closeableTooltip,

                simpleTooltip,

                Ex.CreateSliderWithTooltip(new Vector2(250, 70), trackTexture, handleTexture, handleSize, fillTexture,
                    20, 40f, 0f, 150f, closeableTooltip, new Vector2(400, 190)),

                Ex.CreateToggleSwitch(new Vector2(100, 60), onSwitchTexture, offSwitchTexture, DefaultState.OnSwitch,
                    new Vector2(400, 270)),

                Ex.CreateButton(size, exitTexture, null, penColor, font, Exit_Clicked, new Vector2(400, 350)),

                Ex.CreateTextField(new Vector2(300, 70), texture, penColor, font, new Vector2(400, 480)),

                Ex.CreateLabel(size, "Test Label", penColor, font, new Vector2(400, 580)),

                Ex.CreateSlider(new Vector2(250, 70), trackTexture, handleTexture, handleSize, fillTexture, 20,
                    0f, 0f, 100f, new Vector2(400, 630)),

                Ex.CreateSliderWithTooltip(new Vector2(250, 70), trackTexture, handleTexture, handleSize, fillTexture,
                    20, 40f, 0f, 150f, simpleTooltip, new Vector2(400, 630))
            };

            foreach (var element in _elements)
            {
                AddObject(element);
            }
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.Update();

            InputManager.GetCommands(gameTime, cmd =>
            {
                if (cmd is MainMenuInputCommand.MenuExit)
                {
                    //SwitchState(new GameplayState());
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }
            });
        }

        protected override void UpdateState(GameTime gameTime)
        {
            foreach (var element in _elements)
            {
                if (element.IsActive)
                {
                    element.Update(gameTime);//, InputManager
                }
            }

            var value = (int)((Slider)_elements[7]).Value;
            ((Label)_elements[6]).SetText(value.ToString());
        }

        private void StartStage_Clicked()
        {
            // start the game
            NotifyEvent(new BaseGameStateEvent.GameQuit());
        }

        private void Options_Clicked()
        {
        }

        private void JoinOnline_Clicked()
        {
        }

        private void Exit_Clicked()
        {
            NotifyEvent(new BaseGameStateEvent.GameQuit());
        }
    }
}