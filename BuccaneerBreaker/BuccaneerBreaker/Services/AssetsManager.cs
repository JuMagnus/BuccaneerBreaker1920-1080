using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BuccaneerBreaker
{

    public interface IAssets
    {
        public T Get<T>(string name);
        public Texture2D CreateTransparentTexture(GraphicsDevice graphicsDevice, int width, int height);
    }

    public class AssetsManager : IAssets
    {
        private Dictionary<string, object> _assets = new Dictionary<string, object>();
        private ContentManager _contentManager;


        public AssetsManager(ContentManager cm)
        {
            _contentManager = cm;
            ServicesLocator.Register<IAssets>(this);

            Texture2D texture = new Texture2D(ServicesLocator.Get<SpriteBatch>().GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });
            _assets["OnePixel"] = texture;
        }

        public void Load<T>(string name)
        {
            T asset = _contentManager.Load<T>(name);
            _assets[name] = asset;

        }

        public T Get<T>(string name)
        {
            return (T)_assets[name];
        }

        public Texture2D CreateTransparentTexture(GraphicsDevice graphicsDevice, int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Transparent;
            }

            texture.SetData(data);

            return texture;
        }
    }
}
