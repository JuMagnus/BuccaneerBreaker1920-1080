using BuccaneerBreaker.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BuccaneerBreaker
{
    public class SceneLevelSelection : Scene
    {

        private IAssets _assets = ServicesLocator.Get<IAssets>();
        private IInputs _inputs = ServicesLocator.Get<IInputs>();
        private ISceneManager _sceneManager = ServicesLocator.Get<ISceneManager>();
        private IScreen _screen = ServicesLocator.Get<IScreen>();
        private Ilevels _levels = ServicesLocator.Get<Ilevels>();

        private Sprite _background;
        private int _levelCount;
        private int i;
        private int _startYPosition;
        private List<Button> _buttons;
        private SoundEffect _clickButton;

        public override void Load()
        {
            base.Load();
            _buttons = new List<Button>();
            _clickButton = _assets.Get<SoundEffect>("SoundClic");
            _levelCount = 4;
            _startYPosition = (int)_screen.Center.Y - (_levelCount * 100) / 2;
            _background = new Sprite(Vector2.Zero, _assets.Get<Texture2D>("TexLevelSelection"));

            AddSprite(_background);


            
            int buttonWidth = 275;
            int buttonHeight = 50;

            for (i = 0; i < _levelCount; i++)
            {
                int Y = _startYPosition + i * 100;
                string levelString = "Level " + (i + 1);
                Button levelButton = new Button(new Rectangle((int)_screen.Center.X - buttonWidth / 2, Y, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), levelString);
                _buttons.Add(levelButton);
                AddSprite(levelButton);

            }

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            if (_inputs.IsMouseLeftClicked())
            {
                Vector2 mousePosition = _inputs.GetMousePosition();

                foreach (Button button in _buttons)
                {
                    if (button.colliderBox.Contains(mousePosition))
                    {
                        _levels.currentLevel = _buttons.IndexOf(button);
                        _clickButton.Play();
                        _sceneManager.Load(typeof(SceneGame));
                    }
                }

            }



        }

    }
}
