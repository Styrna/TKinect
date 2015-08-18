using System;
using System.Collections.Generic;
using TKinect.Data;

namespace Kinect.Toolbox.Collectors
{
    public class FrameReadyEventArgs : EventArgs
    {
        public LinkedList<Frame> Frames { get; private set; }

        public FrameReadyEventArgs(LinkedList<Frame> frames)
        {
            Frames = frames;
        }

    }
}