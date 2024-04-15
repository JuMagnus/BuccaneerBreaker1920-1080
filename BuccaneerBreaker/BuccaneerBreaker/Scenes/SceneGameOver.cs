using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace BuccaneerBreaker
{
    public class SceneGameOver : Scene
    {
        private IAssets _assets = ServicesLocator.Get<IAssets>();
        private IInputs _inputs = ServicesLocator.Get<IInputs>();
        private ISceneManager _sceneManager = ServicesLocator.Get<ISceneManager>();
        private IScreen _screen = ServicesLocator.Get<IScreen>();

        private Sprite _background;
        private Button _playAgain;
        private Button _changePaddle;
        private Button _quit;

        private SoundEffect _clickButton;

        public override void Load()
        {
            _background = new Sprite(Vector2.Zero, _assets.Get<Texture2D>("TexGameOver"));
            AddSprite(_background);
            
            
            int buttonWidth = 275;
            int buttonHeight = 50;
            int buttonX = (int)(_screen.Center.X - buttonWidth / 2);
            int startY = (int)(_screen.Center.Y - buttonHeight / 2);

            _playAgain = new Button(new Rectangle(buttonX, startY, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "PLAY AGAIN");
            AddSprite(_playAgain);

            _changePaddle = new Button(new Rectangle(buttonX, startY + 100, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "CHANGE CHARACTER");
            AddSprite(_changePaddle);

            _quit = new Button(new Rectangle(buttonX, startY + 200, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "QUIT");
            AddSprite(_quit);

            _clickButton = _assets.Get<SoundEffect>("SoundClic");
            PlaySong("MusicLoose", false);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_inputs.IsMouseLeftClicked())
            {
                Vector2 mousePosition = _inputs.GetMousePosition();
                _clickButton.Play();
                if (_playAgain.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _sceneManager.Load(typeof(SceneGame));
                }
                else if (_changePaddle.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _sceneManager.Load(typeof(SceneChoosePaddle));
                }
                else if (_quit.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _sceneManager.ExitGame();
                }
            }
        }
    }
}
