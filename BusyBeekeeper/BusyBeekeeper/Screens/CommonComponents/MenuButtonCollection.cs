using System;
using System.Collections;
using System.Collections.Generic;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed class MenuButtonCollection : IEnumerable<MenuButton>
    {
        #region Instance Fields --------------------------------------------------------

        private readonly MenuButton mParentMenuButton;
        private readonly List<MenuButton> mMenuButtons = new List<MenuButton>();
        private ButtonMenuComponent mButtonMenuComponent;

        #endregion

        #region Constructors -----------------------------------------------------------

        public MenuButtonCollection(MenuButton parentMenuButton)
        {
            this.mParentMenuButton = parentMenuButton;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public ButtonMenuComponent ButtonMenuComponent
        {
            get { return this.mButtonMenuComponent; }
            internal set
            {
                if (this.mButtonMenuComponent == value) return;
                this.mButtonMenuComponent = value;

                foreach (var lMenuButton in this)
                {
                    lMenuButton.ButtonMenuComponent = value;
                }
            }
        }

        public MenuButton ParentMenuButton { get { return this.mParentMenuButton; } }
        public int Count { get { return this.mMenuButtons.Count; } }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public MenuButton this[int index]
        {
            get { return this.mMenuButtons[index]; }
        }

        public void Add(MenuButton menuButton)
        {
            if (menuButton == null) throw new ArgumentNullException("menuButton");
            menuButton.ParentMenuButton = this.ParentMenuButton;
            menuButton.ButtonMenuComponent = this.mButtonMenuComponent;
            this.mMenuButtons.Add(menuButton);
            this.ParentMenuButton.Invalidate();
        }

        public bool Remove(MenuButton menuButton)
        {
            if (this.mMenuButtons.Remove(menuButton))
            {
                menuButton.ParentMenuButton = null;
                this.ParentMenuButton.Invalidate();
                return true;
            }
            return false;
        }

        public void Clear()
        {
            if (this.Count == 0) return;
            foreach (var lMenuButton in this) lMenuButton.ParentMenuButton = null;
            this.mMenuButtons.Clear();
            this.ParentMenuButton.Invalidate();
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
