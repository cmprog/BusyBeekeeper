using Microsoft.Xna.Framework.Input;

namespace BusyBeekeeper
{
    internal sealed class InputState
    {
        private KeyboardState mCurrentKeyboardState;
        private KeyboardState mLastKeyboardState;

        private MouseState mCurrentMouseState;
        private MouseState mLastMouseState;

        public bool MouseLeftClickUpHandled { get; set; }

        public void Update()
        {
            this.mLastKeyboardState = this.mCurrentKeyboardState;
            this.mCurrentKeyboardState = Keyboard.GetState();

            this.mLastMouseState = this.mCurrentMouseState;
            this.mCurrentMouseState = Mouse.GetState();

            this.MouseLeftClickUpHandled = false;
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
            return
                !this.MouseLeftClickUpHandled &&
                (this.mLastMouseState.LeftButton == ButtonState.Pressed) &&
                (this.mCurrentMouseState.LeftButton == ButtonState.Released);
        }
    }
}
