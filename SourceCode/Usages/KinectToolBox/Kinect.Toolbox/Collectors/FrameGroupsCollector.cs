using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect.Toolbox.Collectors
{
    public class FrameGroupsCollector : FramesCollector
    {
        public FrameGroupsCollector(KinectSensor kinectSensor, int framesCount = 20)
        {
            _framesCount = framesCount;
            _frames = new LinkedList<Frame>();
            kinectSensor.SkeletonFrameReady += OnSkeletonFrameReady;
        }

        public event FrameReadyHandler FrameReady;
        public delegate void FrameReadyHandler(object sender, FrameReadyEventArgs args);


        public override void OnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                var frame = new Frame(skeletonFrame);

                _frames.AddLast(frame);
                if (_frames.Count == _framesCount)
                {
                    if (FrameReady != null)
                    {
                        FrameReady(this, new FrameReadyEventArgs(_frames));
                        
                    }
                    _frames = new LinkedList<Frame>();
                }
            }
        }
    }
}
