using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox
{
    public class Frame
    {
        public long Timestamp { get; set; }
        public int FrameNumber { get; private set; }
        public TSkeleton[] Skeletons { get; private set; }

        public Frame(TKinect.Data.SkeletonData.TSkeletonFrame skeletonImageFrame)
        {
            FrameNumber = skeletonImageFrame.FrameNumber;
            Timestamp = DateTime.Now.Ticks;

            Skeletons = skeletonImageFrame.Skeletons;
        }
    }
}
