using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Extensions
{
    public static class ObjectExtensions
    {
        public static void SetAndPerform<T>(this T oldValue, ref T newValue, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue)) return;
            oldValue = newValue;
            action();
        }
    }
}
