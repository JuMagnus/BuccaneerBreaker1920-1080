using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace BuccaneerBreaker
{
    public class Timer
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _position;
        private readonly SpriteFont _font;
        private readonly Vector2 _textPosition;
        private string _text;
        public float timeLeft;
        private bool _isActive;

        public Timer(Texture2D texture, Vector2 position, SpriteFont font, float length)
        {
            _texture = texture;
            _position = position;
            _font = font;
            timeLeft = length;
            _textPosition = new Vector2(position.X + 75, position.Y + 15);
            _text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss\.ff");
        }

        private void FormatText()
        {
            _text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss\.ff");
        }

        public void StartStop()
        {
            _isActive = !_isActive;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isActive) return;
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeLeft < 0)
            {
                StartStop();
                timeLeft = 0;
            }
            FormatText();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, _position, Color.White);
            spritebatch.DrawString(_font, _text, _textPosition, Color.Black);
        }
    }
}
