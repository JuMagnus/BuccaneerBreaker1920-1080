using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace BuccaneerBreaker
{
    public abstract class Scene
    {
        public List<Sprite> _sprites = new List<Sprite>();

        public void AddSprite(Sprite spr)
        {
            _sprites.Add(spr);
        }

        public void PauseUnpauseSprite()
        {
            foreach(Sprite spr in _sprites)
            {
                spr.isPaused = !spr.isPaused;
            }
        }

        public void PlaySong(string song, bool repeat)
        {
            Song _song = ServicesLocator.Get<IAssets>().Get<Song>(song);
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = repeat;
        }
        public void PlayLevelSong(Song song, bool repeat)
        {
            Song _song = song;
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = repeat;
        }


        public virtual void Load()
        {

        }

        public virtual void LoadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = _sprites.Count - 1; i >= 0; i--)
            {
                _sprites[i].Update(dt);
                if (_sprites[i].isFree)
                    _sprites.RemoveAt(i);
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public List<T> GetSprites<T>()
        {
            var list = new List<T>();
            foreach (var item in _sprites)
            {
                if(item is T typedItem)
                {
                    list.Add(typedItem);
                }
            }
            return list;
        }

        public virtual void Unload()
        {

        }
    }
}
