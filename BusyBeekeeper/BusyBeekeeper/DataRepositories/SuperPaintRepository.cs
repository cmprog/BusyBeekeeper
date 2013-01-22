using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.DataRepositories
{
    internal sealed class SuperPaintRepository : IMetaObjectRepository<MetaSuperPaint, SuperPaint>
    {
        #region Instance Fields --------------------------------------------------------

        private readonly MetaSuperPaint[] mMetaSuperPaints;

        #endregion

        #region Constructors -----------------------------------------------------------

        public SuperPaintRepository(ContentManager contentManager)
        {
            this.mMetaSuperPaints = contentManager.Load<MetaSuperPaint[]>("Data/SuperPaints");
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public int Count
        {
            get { return this.mMetaSuperPaints.Length; }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public MetaSuperPaint GetMetaObject(int metaId)
        {
            return this.mMetaSuperPaints[metaId];
        }

        public SuperPaint CreateObject(int metaId)
        {
            return this.CreateObject(this.GetMetaObject(metaId));
        }

        public SuperPaint CreateObject(MetaSuperPaint metaObject)
        {
            var lSuperPaint = new SuperPaint();
            lSuperPaint.MetaId = metaObject.Id;
            lSuperPaint.Name = metaObject.Name;
            lSuperPaint.Description = metaObject.Description;
            lSuperPaint.ColorValueA = metaObject.ColorValueA;
            lSuperPaint.ColorValueR = metaObject.ColorValueR;
            lSuperPaint.ColorValueG = metaObject.ColorValueG;
            lSuperPaint.ColorValueB = metaObject.ColorValueB;
            return lSuperPaint;
        }

        #endregion
    }
}
