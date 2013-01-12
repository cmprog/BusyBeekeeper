using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Core
{
    public interface IMetaObjectRepository<TMetaObject, TObject>
    {
        TMetaObject GetMetaObject(int metaId);

        TObject CreateObject(int metaId);
        TObject CreateObject(TMetaObject metaObject);
    }
}
