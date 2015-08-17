using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKinect.Data
{
    [FlagsAttribute]
    public enum TFrameType
    {
        TColorFrame = 1,
        TDepthFrame = 2,
        TSkeletonFrame = 4
    }
}
