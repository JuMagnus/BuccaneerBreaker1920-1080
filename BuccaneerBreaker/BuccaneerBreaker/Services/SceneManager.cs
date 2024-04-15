using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BuccaneerBreaker
{
    public interface ISceneManager
    {
        public void Load(Type sceneType);
        public void ExitGame();
    }

    public sealed class SceneManager : ISceneManager
    {
        private Scene _currentScene;
        private Dictionary<Type, Scene> _scenes = new Dictionary<Type, Scene>();
        private Main _main;

        public SceneManager(Main main)
        {
            ServicesLocator.Register<ISceneManager>(this);
            _main = main;
        }

        public void Register(Scene scene)
        {
            Debug.WriteLine(scene.GetType());
            _scenes.Add(scene.GetType(), scene);
        }

        public void Load(Type sceneType)
        {
            if (_currentScene != null)
            {
                _currentScene.Unload();
                _currentScene = null;

            }
            _currentScene = _scenes[sceneType];
            _currentScene.Load();
        }

        public void Update(GameTime gameTime)
        {
            if (_currentScene != null) _currentScene.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentScene != null) _currentScene.Draw(spriteBatch);
        }

        public void ExitGame()
        {
            _main.Exit();
        }

    }
}
