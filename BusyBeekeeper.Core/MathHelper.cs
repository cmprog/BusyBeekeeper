﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusyBeekeeper.Core
{
    internal static class MathHelper
    {
        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }
    }
}
