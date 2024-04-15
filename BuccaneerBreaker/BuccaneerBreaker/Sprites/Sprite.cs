using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BuccaneerBreaker
{
    public class Sprite
    {
        public Vector2 position;
        protected Texture2D _texture;
        public bool isFree = false;
        public bool isPaused = false;


        public Sprite()
        {
            this.position = Vector2.Zero;
        }
        public Sprite(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this._texture = texture;
        }

        public Sprite(Vector2 position)
        {
            this.position = position;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, position, Color.White);

            DrawUtils.Rectangle(new Vector2(colliderBox.X, colliderBox.Y), colliderBox.Width, colliderBox.Height, Color.Transparent);
        }

        public Rectangle colliderBox => new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);
        

    }
}
