using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKinectTests
{
    public static class Utils
    {
        public static float NextFloat(this Random random)
        {
            double val = random.NextDouble();       // range 0.0 to 1.0
            val -= 0.5;                             // expected range now -0.5 to +0.5
            val *= 2;                               // expected range now -1.0 to +1.0
            return float.MaxValue * (float)val;
        }
    }
}
