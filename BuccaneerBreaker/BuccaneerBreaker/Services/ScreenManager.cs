using Microsoft.Xna.Framework;

namespace BuccaneerBreaker
{
    public interface IScreen
    {
        float Width { get; }
        float Height { get; }

        Vector2 Center { get; }
        Rectangle Bounds { get; }
    }

    internal sealed class ScreenManager : IScreen
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        public ScreenManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            ServicesLocator.Register<IScreen>(this);
        }
        public float Width => _graphicsDeviceManager.PreferredBackBufferWidth;

        public float Height => _graphicsDeviceManager.PreferredBackBufferHeight;


        public Vector2 Center => new Vector2(Width * .5f, Height * .5f);

        public Rectangle Bounds => new Rectangle(0, 0, (int)Width, (int)Height);

    }
}
