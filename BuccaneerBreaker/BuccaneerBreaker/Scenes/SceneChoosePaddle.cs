using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BuccaneerBreaker
{
    public class SceneChoosePaddle : Scene
    {
        private IAssets _assets = ServicesLocator.Get<IAssets>();
        private IInputs _inputs = ServicesLocator.Get<IInputs>();
        private ISceneManager _sceneManager = ServicesLocator.Get<ISceneManager>();
        private IScreen _screen = ServicesLocator.Get<IScreen>();
        private IPaddle _paddle = ServicesLocator.Get<IPaddle>();

        private Sprite _background;
        private Button _cannonBall;
        private Button _gunner;
        private SoundEffect _clickButton;

        public override void Load()
        {
            _background = new Sprite(Vector2.Zero, _assets.Get<Texture2D>("TexPaddleType"));
            AddSprite(_background);

            
            int buttonWidth = 300;
            int buttonHeight = 50;
            int buttonSpacing = 20;

            int centerX = (int)_screen.Center.X;
            int centerY = (int)_screen.Center.Y;

            int cannonBallButtonY = centerY - buttonHeight - buttonSpacing;
            int gunnerButtonY = centerY + buttonSpacing;

            _cannonBall = new Button(new Rectangle(centerX - buttonWidth / 2, cannonBallButtonY, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "CANNONBALL");
            AddSprite(_cannonBall);

            _gunner = new Button(new Rectangle(centerX - buttonWidth / 2, gunnerButtonY, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "GUNNER");
            AddSprite(_gunner);

            _clickButton = _assets.Get<SoundEffect>("SoundClic");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_inputs.IsMouseLeftClicked())
            {
                Vector2 mousePosition = _inputs.GetMousePosition();
                _clickButton.Play();

                if (_cannonBall.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _paddle.currentType = "cannonBall";
                    _sceneManager.Load(typeof(SceneLevelSelection));
                }
                else if (_gunner.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _paddle.currentType = "gunner";
                    _sceneManager.Load(typeof(SceneLevelSelection));
                }
            }
        }
    }
}
