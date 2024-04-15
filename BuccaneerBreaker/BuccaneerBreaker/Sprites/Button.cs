using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuccaneerBreaker
{
    public class Button : Sprite
    {
        public Rectangle area;
        public Color color;
        public SpriteFont font;
        public string text;
        public IAssets assets = ServicesLocator.Get<IAssets>();
        GraphicsDevice graphicsDevice = ServicesLocator.Get<SpriteBatch>().GraphicsDevice;

        public Button(Rectangle area, Color color, SpriteFont font, string text) : base(Vector2.Zero, null)
        {
            this.area = area;
            this.color = color;
            this.font = font;
            this.text = text;
            _texture = assets.CreateTransparentTexture(graphicsDevice, area.Width, area.Height);

        }

        public override void Draw(SpriteBatch spritebatch)
        {
            position = new Vector2(area.X, area.Y);
            Vector2 areaCenter = new Vector2(area.Width/2, area.Height/2);
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(area.X + (area.Width - textSize.X) / 2, area.Y + (area.Height - textSize.Y) / 2);

            DrawUtils.Rectangle(position, area.Width, area.Height, Color.DarkBlue, 4f);
            spritebatch.DrawString(font, text, textPosition, color);

            base.Draw(spritebatch); 
        }

    }
}
