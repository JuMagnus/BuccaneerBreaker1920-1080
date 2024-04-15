using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BuccaneerBreaker
{
    public class CannonBall : Sprite
    {
        private Vector2 _velocity;
        private float _speed = 220f;
        private Rectangle _bounds;
        private float _explosionRadius = 75f;
        private SoundEffect _effect = ServicesLocator.Get<IAssets>().Get<SoundEffect>("SoundBreak");

        public CannonBall(Vector2 position, Rectangle bounds) : base(position)
        {
            _bounds = bounds;
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexCannonball");
            _velocity = new Vector2(0, -1) * _speed;
        }

        private Rectangle GetFuturePosition(float dt)
        {
            Rectangle futurePosition = new Rectangle(
                    (int)(colliderBox.X + _velocity.X * dt),
                    (int)(colliderBox.Y + _velocity.Y * dt),
                    colliderBox.Width,
                    colliderBox.Height
                );
            return futurePosition;
        }

        public void CheckCollisionWithBrick(List<Brick> bricks, float dt)
        {
            Rectangle futurePosition = GetFuturePosition(dt);
            foreach (Brick brick in bricks)
            {
                if (brick.isFree)
                    continue;
                if (futurePosition.Intersects(brick.colliderBox))
                {
                    brick.isFree = true;
                    
                    // Explosion du boulet et destruction des briques dans l AOE
                    foreach (Brick nearbyBrick in bricks)
                    {
                        if (!nearbyBrick.isFree && brick != nearbyBrick)
                        {
                            float distance = Vector2.Distance(new Vector2(brick.colliderBox.Center.X, brick.colliderBox.Center.Y),
                                                              new Vector2(nearbyBrick.colliderBox.Center.X, nearbyBrick.colliderBox.Center.Y));
                            if (distance <= _explosionRadius)
                            {
                                nearbyBrick.isFree = true;
                                _effect.Play();
                            }
                        }
                    }
                    isFree = true;
                    break;
                }

            }

        }

        public override void Update(float dt)
        {
            if (!isPaused)
            {
                position += _velocity * dt;
                base.Update(dt);

                if (position.Y < _bounds.Top)
                {
                    isFree = true;
                }

            }


        }

    }
}
