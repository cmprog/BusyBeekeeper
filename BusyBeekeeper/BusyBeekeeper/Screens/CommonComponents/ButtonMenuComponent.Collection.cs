using System;
using System.Collections;
using System.Collections.Generic;

namespace BusyBeekeeper.Screens.CommonComponents
{
    partial class ButtonMenuComponent
    {
        public sealed class Collection : IEnumerable<MenuButton>
        {
            #region Instance Fields --------------------------------------------------------

            private readonly ButtonMenuComponent mButtonMenuComponent;
            private readonly List<MenuButton> mMenuButtons = new List<MenuButton>();

            #endregion

            #region Constructors -----------------------------------------------------------

            public Collection(ButtonMenuComponent buttonMenuComponent)
            {
                if (buttonMenuComponent == null) throw new ArgumentNullException("buttonMenuComponent");
                this.mButtonMenuComponent = buttonMenuComponent;
            }

            #endregion

            #region Instance Properties ----------------------------------------------------

            public ButtonMenuComponent ButtonMenuComponent
            {
                get { return this.mButtonMenuComponent; }
            }

            public int Count
            {
                get { return this.mMenuButtons.Count; }
            }

            #endregion

            #region Instance Methods -------------------------------------------------------
            
            public MenuButton this[int index]
            {
                get { return this.mMenuButtons[index]; }
            }

            public void Add(MenuButton menuButton)
            {
                if (menuButton == null) throw new ArgumentNullException("menuButton");
                menuButton.ParentMenuButton = null;
                menuButton.ButtonMenuComponent = this.ButtonMenuComponent;
                this.mMenuButtons.Add(menuButton);
                this.ButtonMenuComponent.Invalidate();
            }

            public bool Remove(MenuButton menuButton)
            {
                if (this.mMenuButtons.Remove(menuButton))
                {
                    menuButton.ParentMenuButton = null;
                    this.ButtonMenuComponent.Invalidate();
                    return true;
                }
                return false;
            }

            public void Clear()
            {
                if (this.Count == 0) return;
                foreach (var lMenuButton in this) lMenuButton.ParentMenuButton = null;
                this.mMenuButtons.Clear();
                this.ButtonMenuComponent.Invalidate();
            }

            public IEnumerator<MenuButton> GetEnumerator()
            {
                return this.mMenuButtons.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }
    }
}
