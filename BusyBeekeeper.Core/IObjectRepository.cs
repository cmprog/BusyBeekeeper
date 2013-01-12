using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Core
{
    public interface IObjectRepository<TObject>
    {
        int Count { get; }
        TObject CreateObject(int id);
    }
}
