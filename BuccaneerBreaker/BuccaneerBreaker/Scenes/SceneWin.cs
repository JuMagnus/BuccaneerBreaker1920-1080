using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BuccaneerBreaker
{
    public class SceneWin : Scene
    {
        private IAssets _assets = ServicesLocator.Get<IAssets>();
        private IInputs _inputs = ServicesLocator.Get<IInputs>();
        private ISceneManager _sceneManager = ServicesLocator.Get<ISceneManager>();
        private IScreen _screen = ServicesLocator.Get<IScreen>();

        private Sprite _background;
        private Button _playAgain;
        private Button _selectLevel;
        private Button _mainMenu;

        private SoundEffect _clickButton;


        public override void Load()
        {
            _background = new Sprite(Vector2.Zero, _assets.Get<Texture2D>("TexWin"));
            AddSprite(_background);

            
            int buttonWidth = 275;
            int buttonHeight = 50;
            int buttonX = (int)_screen.Center.X - buttonWidth / 2;

            _playAgain = new Button(new Rectangle(buttonX, (int)_screen.Center.Y - 50, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "PLAY AGAIN");
            AddSprite(_playAgain);

            _selectLevel = new Button(new Rectangle(buttonX, (int)_screen.Center.Y + 50, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "SELECT LEVEL");
            AddSprite(_selectLevel);

            _mainMenu = new Button(new Rectangle(buttonX, (int)_screen.Center.Y + 150, buttonWidth, buttonHeight), Color.White, _assets.Get<SpriteFont>("FontTradeWind"), "MENU PRINCIPAL");
            AddSprite(_mainMenu);

            _clickButton = _assets.Get<SoundEffect>("SoundClic");
            PlaySong("MusicWin", false);

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
                else if (_selectLevel.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _sceneManager.Load(typeof(SceneLevelSelection));
                }
                else if (_mainMenu.colliderBox.Contains(mousePosition.X, mousePosition.Y))
                {
                    _sceneManager.Load(typeof(SceneMenu));
                }
            }



        }
    }
}
