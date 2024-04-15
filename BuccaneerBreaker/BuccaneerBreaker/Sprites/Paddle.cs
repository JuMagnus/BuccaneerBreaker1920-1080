using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static BuccaneerBreaker.Delegates;

namespace BuccaneerBreaker
{
    public class Paddle : Sprite
    {
        private float _speed = 300;
        private float _lerpSpeed = 0.1f;
        private Vector2 _targetPosition;
        private Rectangle _bounds;
        private Rectangle _specialMax;
        private Rectangle _specialCurrent;
        private float _specialFillingSpeed = 15f;
        private float _accumulatedFillDistance;
        private string _paddleType;
        private string _XText;
        private Vector2 _XTextSize;
        private Vector2 _XTextPosition;
        private SpriteFont _font = ServicesLocator.Get<IAssets>().Get<SpriteFont>("FontTradeWind");

        public Paddle(Rectangle bounds) : base()
        {
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexPaddle");
            _paddleType = ServicesLocator.Get<IPaddle>().currentType;
            _bounds = bounds;

            position = new Vector2(bounds.Center.X - colliderBox.Center.X, bounds.Bottom - colliderBox.Height);
            _targetPosition = position;
            _specialMax = new Rectangle(5, 60, 175, 25);
            _specialCurrent = new Rectangle(5, 60, 87, 25);
            _XText = "PRESS X";
            _XTextSize = _font.MeasureString(_XText);
            _XTextPosition = new Vector2(_specialMax.X + (_specialMax.Width - _XTextSize.X) / 2, _specialMax.Y + (_specialMax.Height - _XTextSize.Y) / 2);
        }

        public event CannonBallLaunchedEventHandler CannonBallLaunched;
        public event BurstEventHandler Burst;

        protected virtual void OnCannonBallLaunched(EventArgs e)
        {
            CannonBallLaunched?.Invoke(this, e);
        }
        public void LaunchCannonBall()
        {
            OnCannonBallLaunched(EventArgs.Empty);
        }


        protected virtual void OnBurst(EventArgs e)
        {
            Burst?.Invoke(this, e);
        }
        public void LaunchBurst()
        {
            OnBurst(EventArgs.Empty);
        }



        public override void Update(float dt)
        {
            if (!isPaused)
            {
                var inputs = ServicesLocator.Get<IInputs>();
                var direction = Vector2.Zero;


                if (_specialCurrent.Width < _specialMax.Width)
                {
                    float fillSpeed = _specialFillingSpeed * dt;
                    _accumulatedFillDistance += fillSpeed;
                    if (_accumulatedFillDistance > 1)
                    {
                        _specialCurrent.Width += (int)_accumulatedFillDistance;
                        _accumulatedFillDistance = 0;
                    }

                    if (_specialCurrent.Width > _specialMax.Width)
                    {
                        _specialCurrent.Width = _specialMax.Width;
                    }
                }


                if (inputs.IsKeyPressed(Keys.Left)) direction = -Vector2.UnitX;
                else if (inputs.IsKeyPressed(Keys.Right)) direction = Vector2.UnitX;

                _targetPosition += direction * _speed * dt;
                position = Vector2.Lerp(position, _targetPosition, _lerpSpeed);

                if (position.X < _bounds.Left)
                {
                    position.X = _bounds.Left;
                    _targetPosition = position;
                }
                else if (position.X + colliderBox.Width > _bounds.Right)
                {
                    position.X = _bounds.Right - colliderBox.Width;
                    _targetPosition = position;
                }


                if (inputs.IsJustPressed(Keys.X) && _specialCurrent == _specialMax)
                {
                    if (_paddleType == "cannonBall")
                    {
                        LaunchCannonBall();
                    }
                    else if (_paddleType == "gunner")
                    {
                        LaunchBurst();
                    }
                    _specialCurrent.Width = 0;
                }

                base.Update(dt);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawUtils.DrawFilledRectangle(spriteBatch, _specialCurrent, Color.Red);
            DrawUtils.Rectangle(new Vector2(_specialMax.X, _specialMax.Y), _specialMax.Width, _specialMax.Height, Color.Black, 2);
            if (_specialCurrent == _specialMax)
                spriteBatch.DrawString(_font, _XText, _XTextPosition, Color.Black);
            base.Draw(spriteBatch);
        }
    }
}
