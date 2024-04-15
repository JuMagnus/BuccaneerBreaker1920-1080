using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BuccaneerBreaker
{
    public class Life : Sprite
    {

        public Life(Vector2 position) : base(position)
        {
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexFullLife");
        }

    }
}
