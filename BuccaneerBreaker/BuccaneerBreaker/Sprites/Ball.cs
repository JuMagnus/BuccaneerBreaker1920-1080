using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BuccaneerBreaker
{
    
    public class Ball : Sprite
    {
        private Vector2 _velocity;
        private float _speed = 250f;
        private Rectangle _bounds;
        private int _bouncingsBeforeDestroy = 6; 
        private int _decrease; //sert à rendre les balles du multiball éphémères
        private SoundEffect _effect = ServicesLocator.Get<IAssets>().Get<SoundEffect>("SoundBreak");

        //Balle simple
        public Ball(Vector2 position, Rectangle bounds, int decrease) : base(position)
        {
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexBall");
            _velocity = new Vector2(0.75f, -1) * _speed;
            _bounds = bounds;
            _decrease = decrease;
        }

        //balles du multiball
        public Ball(Vector2 position, Rectangle bounds, int decrease, float vx, float vy) : base(position)
        {
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexMultiBall");
            _velocity = new Vector2(vx, vy) * _speed;
            _bounds = bounds;
            _decrease = decrease;
        }

        public override void Update(float dt)
        {
            if (!isPaused)
            {
                position += _velocity * dt;
                base.Update(dt);
            }
            
            
        }

        public void BouncingToBounds(ref int _currentLife, ref bool _isBallLaunched)
        {
            // Y a certainement moyen de rendre ça beaucoup plus élégant. je n'aime pas les chaines d'if
            if (position.Y > _bounds.Bottom - colliderBox.Height && _bouncingsBeforeDestroy == 6)
            {
                isFree = true;
                _currentLife = _currentLife - 1;
                _isBallLaunched = false;
            }
            else if (position.Y < _bounds.Top )
            {
                position.Y = _bounds.Top;
                _velocity.Y *= -1;
                _bouncingsBeforeDestroy = _bouncingsBeforeDestroy - _decrease ;
            }
            if (position.X > _bounds.Right - colliderBox.Width)
            {
                position.X = _bounds.Right - colliderBox.Width;
                _velocity.X *= -1;
                _bouncingsBeforeDestroy = _bouncingsBeforeDestroy - _decrease;
            }
            else if (position.X < _bounds.Left)
            {
                position.X = _bounds.Left;
                _velocity.X *= -1;
                _bouncingsBeforeDestroy = _bouncingsBeforeDestroy - _decrease;
            }

            if(_bouncingsBeforeDestroy == 0)
            {
                isFree = true ;
            }
            
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

        private bool Bounce(Sprite sprite, Rectangle futurePosition)
        {
            if ( sprite.colliderBox.Intersects(futurePosition))
            {
                Point delta = colliderBox.Center - sprite.colliderBox.Center;
                float ratioX = Math.Abs(delta.X) / (sprite.colliderBox.Width/2 + futurePosition.Width/2);
                float ratioY = Math.Abs(delta.Y) / (sprite.colliderBox.Height / 2 + futurePosition.Height / 2);
                if (ratioX > ratioY)
                    _velocity.X *= -1f;
                else
                    _velocity.Y *= -1f;

                return true;
            }
            else 
                return false;
        }

        public void CheckCollisionWithPaddle(Paddle paddle, float dt)
        {
            Bounce(paddle, GetFuturePosition(dt));
        }

        //J'ai tenté une gestion de l'angle en fonction de la position de la balle par rapport au paddle
        //La balle glitch trop souvent.
        // Je laisse le code pour le reprendre un jour prochain 

        //public void CheckCollisionWithPaddle(Paddle paddle, float dt)
        //{
        //    Rectangle futurePosition = GetFuturePosition(dt);

        //    if (Bounce(paddle, futurePosition))
        //    {
        //        float collisionPosition = (position.X + colliderBox.Width / 2) - (paddle.position.X + paddle.colliderBox.Width / 2);
        //        float normalizedCollisionPosition = MathHelper.Clamp(collisionPosition / (paddle.colliderBox.Width / 2), -0.75f, 0.75f);
        //        float bounceAngle = normalizedCollisionPosition * MathHelper.Pi / 2;
        //        Vector2 newVelocity = Vector2.Transform(_velocity, Matrix.CreateRotationZ(bounceAngle));
        //        _velocity = newVelocity;
        //    }
        //}

        public void CheckCollisionWithBrick(List<Brick> bricks, float dt)
        {
            Rectangle futurePosition = GetFuturePosition(dt);
            foreach (Brick brick in bricks)
            {
                if(brick.isFree)
                    continue;
                  
                if (Bounce(brick, futurePosition))
                {
                    _effect.Play();    
                    brick.isFree = true;
                    _bouncingsBeforeDestroy = _bouncingsBeforeDestroy - _decrease;
                    break;
                }
            }

        }
    }
}
