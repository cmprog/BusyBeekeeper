using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Screens.CommonComponents
{
    partial class InventoryItemListComponent
    {
        public sealed class ItemCollection
        {
            #region Instance Fields --------------------------------------------------------

            private readonly InventoryItemListComponent mListComponent;
            private readonly List<InventoryItem> mItems = new List<InventoryItem>();

            #endregion

            #region Constructors -----------------------------------------------------------

            public ItemCollection(InventoryItemListComponent listComponent)
            {
                this.mListComponent = listComponent;
            }

            #endregion

            #region Instance Properties ----------------------------------------------------

            public int Count
            {
                get { return this.mItems.Count; }
            }

            #endregion

            #region Instance Methods -------------------------------------------------------

            public InventoryItem this[int index]
            {
                get { return this.mItems[index]; }
            }

            public void Add(InventoryItem inventoryItem)
            {
                this.mItems.Add(inventoryItem);
                this.mListComponent.UpdateNavigationInformation();
            }

            public void AddRange(IEnumerable<InventoryItem> inventoryItems)
            {
                this.mItems.AddRange(inventoryItems);
                this.mListComponent.UpdateNavigationInformation();
            }

            public void Clear()
            {
                this.mItems.Clear();
                this.mListComponent.UpdateNavigationInformation();
            }

            #endregion
        
        }
    }
}
