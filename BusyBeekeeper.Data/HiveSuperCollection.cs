using System;
using System.Collections;
using System.Collections.Generic;

namespace BusyBeekeeper.Data
{
    public sealed class HiveSuperCollection : IEnumerable<Super>
    {
        #region Instance Fields --------------------------------------------------------

        private readonly BeeHive mBeeHive;
        private readonly List<Super> mSupers = new List<Super>();

        private int mBroodInsertionIndex;

        #endregion

        #region Constructors -----------------------------------------------------------

        internal HiveSuperCollection(BeeHive beeHive)
        {
            this.mBeeHive = beeHive;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public int Count
        {
            get { return this.mSupers.Count; }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public Super this[int index]
        {
            get { return this.mSupers[index]; }
        }

        public void Add(Super super, SuperType type)
        {
            if (super == null) throw new ArgumentNullException("super");
            if (type == SuperType.Unassigned) throw new ArgumentOutOfRangeException("type", type, "Cannot be unassigned.");

            super.Type = type;
            super.BeeHive = this.mBeeHive;

            if (type == SuperType.BroodChamber)
            {
                this.mSupers.Insert(this.mBroodInsertionIndex, super);
                this.mBroodInsertionIndex++;
            }
            else
            {
                this.mSupers.Add(super);
            }
        }

        public bool Remove(Super super)
        {
            if (this.mSupers.Remove(super))
            {
                if (super.Type == SuperType.BroodChamber)
                {
                    super.Type = SuperType.Unassigned;
                    this.mBroodInsertionIndex--;
                }
                return true;
            }
            return false;
        }

        public IEnumerator<Super> GetEnumerator()
        {
            return this.mSupers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
