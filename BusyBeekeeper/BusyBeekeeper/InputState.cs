using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace BusyBeekeeper
{
    internal sealed class InputState
    {
        private KeyboardState mCurrentKeyboardState;
        private KeyboardState mLastKeyboardState;

        private MouseState mCurrentMouseState;
        private MouseState mLastMouseState;

        public void Update()
        {
            this.mLastKeyboardState = this.mCurrentKeyboardState;
            this.mCurrentKeyboardState = Keyboard.GetState();

            this.mLastMouseState = this.mCurrentMouseState;
            this.mCurrentMouseState = Mouse.GetState();
        }

        public KeyboardState CurrentKeyboardState
        {
            get { return this.mCurrentKeyboardState; }
        }

        public KeyboardState LastKeyboardState
        {
            get { return this.mLastKeyboardState; }
        }

        public MouseState CurrentMouseState
        {
            get { return this.mCurrentMouseState; }
        }

        public MouseState LastMouseState
        {
            get { return this.mLastMouseState; }
        }

        public bool MouseLeftDown()
        {
            return (this.mCurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool MouseLeftUp()
        {
            return (this.mCurrentMouseState.LeftButton == ButtonState.Released);
        }

        public bool MouseLeftClickDown()
        {
            return (this.mLastMouseState.LeftButton == ButtonState.Released)
                && (this.mCurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool MouseLeftClickUp()
        {
            return (this.mLastMouseState.LeftButton == ButtonState.Pressed)
                && (this.mCurrentMouseState.LeftButton == ButtonState.Released);
        }
    }
}
