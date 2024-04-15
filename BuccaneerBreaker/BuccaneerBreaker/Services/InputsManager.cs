using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BuccaneerBreaker
{
    public interface IInputs
    {
        bool IsJustPressed(Keys key);
        bool IsKeyPressed(Keys key);
        bool IsMouseLeftClicked();
        Vector2 GetMousePosition();

    }

    public sealed class InputsManager : IInputs
    {
        private KeyboardState _oldKeyboardState;
        private MouseState _oldMouseState;


        public InputsManager()
        {
            ServicesLocator.Register<IInputs>(this);
        }

        public void UpdateKeyboardState()
        {
            _oldKeyboardState = Keyboard.GetState();
        }

        public bool IsJustPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && _oldKeyboardState.IsKeyUp(key);
        }


        public bool IsKeyPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public void UpdateMouseState()
        {
            _oldMouseState = Mouse.GetState();
        }

        
        public bool IsMouseLeftClicked()
        {
            MouseState currentMouseState = Mouse.GetState();
            bool leftButtonPressed = currentMouseState.LeftButton == ButtonState.Pressed;
            bool leftButtonWasReleased = _oldMouseState.LeftButton == ButtonState.Released;

            _oldMouseState = currentMouseState;

            return leftButtonPressed && leftButtonWasReleased;
        }
        public Vector2 GetMousePosition()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }
    }
}
