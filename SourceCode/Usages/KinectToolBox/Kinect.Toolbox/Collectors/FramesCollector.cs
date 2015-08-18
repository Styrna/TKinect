using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Console;
using TKinect;
using TKinect.Data.SkeletonData;
using TKinect.Data;

namespace Kinect.Toolbox.Collectors
{
    public class FramesCollector
    {
        protected int _framesCount;
        protected LinkedList<Frame> _frames;
        public event FrameReadyHandler FrameReady;
        public delegate void FrameReadyHandler(object sender, FrameReadyEventArgs args);
        

        public FramesCollector(TKinect.TKinect kinect, int framesCount = 20)
        {
            _framesCount = framesCount;
            _frames = new LinkedList<Frame>();


            kinect.SkeletonFrameReady += OnSkeletonImageFrameReady;
        }

        public FramesCollector()
        {
        }


        public IEnumerable<Frame> GetFrames(long minimalFrameTimestamp = 0)
        {
            return _frames.Where(a => a.Timestamp >= minimalFrameTimestamp);
        }

        public virtual void OnSkeletonImageFrameReady(object sender, TSkeletonFrame skeletonImageFrame)
        {
            if (skeletonImageFrame != null)
            {
                var frame = new Frame(skeletonImageFrame);

                _frames.AddLast(frame);
                if (_frames.Count == _framesCount)
                {
                    _frames.RemoveFirst();
                }

                if (FrameReady != null)
                {
                    FrameReady(this, new FrameReadyEventArgs(_frames));
                }
            }
        }
    }
}
