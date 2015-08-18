using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKinect.Data;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class DummyHandAnalyzer : FramesAnalyzer
    {
        public DummyHandAnalyzer(FramesCollector framesCollector)
            : base(framesCollector)
        {
        }

        public event Kinect.Toolbox.Analyzers.HandInFrontAnalyzer.MovementDetectedHandler MovementDetected;

        

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            var frames = _framesCollector.GetFrames(LastFrameTimestamp).ToList();
            if (frames.Count == 0)
            {
                return;
            }

            var LeftHandPosHistory = frames.Select(a => a.GetNearestSkeleton().Joints.FirstOrDefault(b => b.JointType == TJointType.HandLeft)).ToList();

            var transitionVector = new Vector3() { X = LeftHandPosHistory.Last().Position.X - LeftHandPosHistory[0].Position.X, Y = LeftHandPosHistory.Last().Position.Y - LeftHandPosHistory[0].Position.Y };

            Raise(() => RaiseEvent(frames, transitionVector, 0, ControlType.RIGHT_HAND));
        }


        protected void RaiseEvent(List<Frame> frames, Vector3 calculateDisplacementVector, long duration, ControlType controlType)
        {
            if(MovementDetected != null)
            {
                MovementDetected(this, new MovementEventArgs(calculateDisplacementVector, duration, TJointType.HandLeft, controlType));
            }
        }
    }
}
