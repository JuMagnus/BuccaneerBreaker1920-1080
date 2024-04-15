using BuccaneerBreaker.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace BuccaneerBreaker
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputsManager _inputs;
        private SceneManager _sceneManager;
        private ScreenManager _screen;
        private AssetsManager _assets;
        private LevelsManager _levels;
        private PaddleTypeManager _paddleType;

        public void Quit()
        {
            this.Exit();
        }


        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 760;
        }

        protected override void Initialize()
        {

            _inputs = new InputsManager();
            _sceneManager = new SceneManager(this);
            _screen = new ScreenManager(_graphics);
            _levels = new LevelsManager();
            _paddleType = new PaddleTypeManager();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ServicesLocator.Register<SpriteBatch>(_spriteBatch);

            _assets = new AssetsManager(Content);

            _assets.Load<Texture2D>("TexDawn");
            _assets.Load<Texture2D>("TexTitle");
            _assets.Load<Texture2D>("TexSkullIsland");
            _assets.Load<Texture2D>("TexSunset");
            _assets.Load<Texture2D>("TexTwilight");
            _assets.Load<Texture2D>("TexPaddle");
            _assets.Load<Texture2D>("TexBrickBrown");
            _assets.Load<Texture2D>("TexBall");
            _assets.Load<Texture2D>("TexMultiBall");
            _assets.Load<Texture2D>("TexFullLife");
            _assets.Load<Texture2D>("TexBanner");
            _assets.Load<Texture2D>("TexGameOver");
            _assets.Load<Texture2D>("TexWin");
            _assets.Load<Texture2D>("TexLevelSelection");
            _assets.Load<Texture2D>("TexCannonball");
            _assets.Load<Texture2D>("TexPaddleType");

            _assets.Load<SpriteFont>("FontTradeWind");

            _assets.Load<Song>("MusicBucaneers");
            _assets.Load<Song>("MusicGame_1");
            _assets.Load<Song>("MusicGame_2");
            _assets.Load<Song>("MusicLoose");
            _assets.Load<Song>("MusicWin");
            _assets.Load<Song>("MusicTitle");
            
            _assets.Load<SoundEffect>("SoundBreak");
            _assets.Load<SoundEffect>("SoundClic");
            _assets.Load<SoundEffect>("SoundShoot");
            _assets.Load<SoundEffect>("SoundMultiShoot");

            _sceneManager.Register(new SceneMenu());
            _sceneManager.Register(new SceneGame());
            _sceneManager.Register(new SceneGameOver());
            _sceneManager.Register(new SceneChoosePaddle());
            _sceneManager.Register(new SceneLevelSelection());
            _sceneManager.Register(new SceneWin());
            

            _levels.LoadAllLevels("Levels");

            _sceneManager.Load(typeof(SceneMenu));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _sceneManager.Update(gameTime);
            _inputs.UpdateKeyboardState();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _sceneManager.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
