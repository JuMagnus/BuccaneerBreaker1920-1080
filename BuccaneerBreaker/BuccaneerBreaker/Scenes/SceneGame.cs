using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using BuccaneerBreaker.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace BuccaneerBreaker
{
    public class SceneGame : Scene
    {
        private Ilevels _levels = ServicesLocator.Get<Ilevels>();
        private ISceneManager _sceneManager = ServicesLocator.Get<ISceneManager>();
        private IInputs _inputs = ServicesLocator.Get<IInputs>();
        private IAssets _assets = ServicesLocator.Get<IAssets>();
        private IScreen _screen = ServicesLocator.Get<IScreen>();

        private Sprite _background;
        private Rectangle _gameBox;
        private Paddle _paddle;
        private bool _isBallLaunched;
        private int _column;
        private int _row;
        private int _maxLife = 5;
        private int _currentLife;
        private Timer _timer;
        private SpriteFont _font;
        private string _gameState;
        private bool _isPaused;
        private Song _music;
        private string _pauseText;
        private Vector2 _pauseTextSize;
        private Vector2 _pauseTextPosition;
        private SoundEffect _shoot;
        private string _shootText;
        private Vector2 _shootTextSize;
        private Vector2 _shootTextPosition;


        private List<Brick> _bricks = new List<Brick>();
        private List<Ball> _balls = new List<Ball>();
        private List<Paddle> _paddles = new List<Paddle>();
        private List<CannonBall> _cannonBalls = new List<CannonBall>();
        private List<Life> _lives = new List<Life>();

        private void AddBricks(Rectangle bounds, int column, int row)
        {
            Brick brick = new Brick(Vector2.Zero);
            brick.position = new Vector2(
                (bounds.X + (row * brick.colliderBox.Width)),
                (bounds.Y + (column * brick.colliderBox.Height))
                );
            AddSprite(brick);
        }

        private void GetLevel()
        {
            var level = _levels.GetLevel(_levels.currentLevel);

            for (_column = 0; _column < level.bricks.Count; _column++)
            {
                for (_row = 0; _row < level.bricks[_column].Count; _row++)
                {
                    if (level.bricks[_column][_row] > 0)
                    {
                        AddBricks(_gameBox, _column, _row);
                    }
                }
            }
        }

        private void LaunchBall(int decrease)
        {
            _shoot.Play();
            Vector2 _launchPosition = new Vector2(_paddle.position.X + _paddle.colliderBox.Width / 2, _paddle.position.Y - _paddle.colliderBox.Height);
            AddSprite(new Ball(_launchPosition, _gameBox, decrease));
        }

        private void MultiBall(int decrease, float vx, float vy)
        {
            Vector2 _launchPosition = new Vector2(_paddle.position.X + _paddle.colliderBox.Width / 2, _paddle.position.Y - _paddle.colliderBox.Height);
            AddSprite(new Ball(_launchPosition, _gameBox, decrease, vx, vy));
        }

        private async Task Burst()
        {
            float speed = 1.0f;
            float[] velocitiesX = { -0.75f, -1f, 1f, 0.75f };
            float[] velocitiesY = { -1f, -2f, -2f, -1f };
            int decrease = 1;
            for (int i = 0; i < 4; i++)
            {
                _shoot.Play();
                float magnitude = (float)Math.Sqrt(velocitiesX[i] * velocitiesX[i] + velocitiesY[i] * velocitiesY[i]);
                velocitiesX[i] *= speed / magnitude;
                velocitiesY[i] *= speed / magnitude;
                MultiBall(decrease, velocitiesX[i], velocitiesY[i]);
                await Task.Delay(100);
            }
        }

        private void OnBurst(object source, EventArgs args)
        {
            Burst();
        }

        private void OnCannonBallLaunched(object source, EventArgs args)
        {
            Vector2 launchPosition = new Vector2(_paddle.position.X + _paddle.colliderBox.Width / 2 - 13, _paddle.position.Y - _paddle.colliderBox.Height);
            AddSprite(new CannonBall(launchPosition, _gameBox));
            _shoot.Play();
        }

        private void UpdateLife(int life)
        {
            _lives = GetSprites<Life>();
            if (_lives.Count > 0)
            {
                foreach (Life item in _lives)
                {
                    item.isFree = true;
                }
            }
            for (int i = 0; i < life; i++)
            {
                Texture2D texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexFullLife");
                Vector2 _lifePosition = new Vector2(5 + i * texture.Width, 20);
                Life _life = new Life(_lifePosition);
                AddSprite(_life);
            }
        }


        public override void Load()
        {
            IScreen screen = ServicesLocator.Get<IScreen>();
            Rectangle screenBox = screen.Bounds;

            _gameBox = new Rectangle(25, 100, screenBox.Width - 50, screenBox.Height - 150);

            var level = _levels.GetLevel(_levels.currentLevel);

            _background = new Sprite(Vector2.Zero, _assets.Get<Texture2D>(level.texture));
            _font = _assets.Get<SpriteFont>("FontTradeWind");

            _pauseText = "PAUSE";
            _pauseTextSize = _font.MeasureString(_pauseText); ;
            _pauseTextPosition = new Vector2((_screen.Width - _pauseTextSize.X) / 2, (_screen.Height - _pauseTextSize.Y) / 2);
            _shootText = "PRESS SPACE TO SHOOT";
            _shootTextSize = _font.MeasureString(_shootText); ;
            _shootTextPosition = new Vector2((_screen.Width - _shootTextSize.X) / 2, (_screen.Height - _shootTextSize.Y) / 2);

            _timer = new Timer(_assets.Get<Texture2D>("TexBanner"), new Vector2(200, 13), _font, 90f);
            _timer.StartStop();
            _currentLife = _maxLife;

            _music = _assets.Get<Song>(level.music);
            _shoot = _assets.Get<SoundEffect>("SoundShoot");

            PlayLevelSong(_music, true);

            AddSprite(_background);
            GetLevel();
            _paddle = new Paddle(_gameBox);
            AddSprite(_paddle);

            _paddle.CannonBallLaunched += OnCannonBallLaunched;
            _paddle.Burst += OnBurst;


            _isBallLaunched = false;
            _gameState = "play";

            base.Load();
        }

        public override void Update(GameTime gameTime)
        {

            switch (_gameState)
            {
                case "play":
                    _isPaused = false;
                    break;
                case "pause":
                    _isPaused = true;
                    break;
                case "win":
                    _sceneManager.Load(typeof(SceneWin));
                    break;
                case "gameOver":
                    _sceneManager.Load(typeof(SceneGameOver));
                    break;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_isPaused == false)
            {
                _timer.Update(gameTime);
                UpdateLife(_currentLife);

                if (_inputs.IsJustPressed(Keys.Space))
                {
                    if (!_isBallLaunched)
                    {
                        LaunchBall(0);
                        _isBallLaunched = true;
                    }
                }


                _bricks = GetSprites<Brick>();
                _balls = GetSprites<Ball>();
                _paddles = GetSprites<Paddle>();
                _cannonBalls = GetSprites<CannonBall>();

                foreach (var ball in _balls)
                {
                    ball.CheckCollisionWithPaddle(_paddles[0], dt);
                    ball.CheckCollisionWithBrick(_bricks, dt);
                    ball.BouncingToBounds(ref _currentLife, ref _isBallLaunched);
                }

                foreach (var cannonball in _cannonBalls)
                {
                    cannonball.CheckCollisionWithBrick(_bricks, dt);
                }
                if (_bricks.Count == 0)
                {
                    _gameState = "win";
                }
                if (_bricks.Count > 0 && (_currentLife == 0 || _timer.timeLeft == 0))
                {
                    _gameState = "gameOver";
                }

            }

            if (_inputs.IsJustPressed(Keys.Enter))
            {
                if (_gameState == "play")
                {
                    _gameState = "pause";
                    PauseUnpauseSprite();
                }
                else if (_gameState == "pause")
                {
                    _gameState = "play";
                    PauseUnpauseSprite();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            _timer.Draw(spriteBatch);
            DrawUtils.Rectangle(new Vector2(_gameBox.X, _gameBox.Y), _gameBox.Width, _gameBox.Height, Color.Black, 2);

            if (_isPaused)
                spriteBatch.DrawString(_font, _pauseText, _pauseTextPosition, Color.OrangeRed);
            if (!_isBallLaunched)
                spriteBatch.DrawString(_font, _shootText, _shootTextPosition, Color.Red);

        }

        public override void Unload()
        {
            base.Unload();
            foreach (Brick brick in _bricks)
            {
                brick.isFree = true;
            }
            foreach (Ball ball in _balls)
            {
                ball.isFree = true;
            }
            foreach (Paddle paddle in _paddles)
            {
                paddle.isFree = true;
            }
            foreach (CannonBall cannonBall in _cannonBalls)
            {
                cannonBall.isFree = true;
            }
        }
    }
}
