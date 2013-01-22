using System;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal class Button : Label
    {
        #region Instance Fields --------------------------------------------------------

        #endregion

        #region Constructors -----------------------------------------------------------

        public Button()
        {
            this.IsEnabled = true;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public event Action<Button> Click;
        public bool IsEnabled { get; set; }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void HandleInput(InputState inputState)
        {
            if (!this.IsVisible || !this.IsEnabled) return;

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    var lClickHandler = this.Click;
                    if (lClickHandler != null)
                    {
                        lClickHandler(this);
                    }
                    inputState.MouseLeftClickUpHandled = true;
                }
            }
        }

        #endregion
    }
}
