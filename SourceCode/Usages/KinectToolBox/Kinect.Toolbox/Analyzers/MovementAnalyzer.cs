using System.Collections.Generic;
using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using Console;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class MovementAnalyzer : FramesAnalyzer
    {
        protected TJointType _joint;

        public delegate void MovementDetectedHandler(object sender, MovementEventArgs args);
        public event MovementDetectedHandler MovementDetected;
        public int MinimalPeriodBetweenGestures { get; set; }
        public float MinimalVectorLength { get; set; }

        public MovementAnalyzer(FramesCollector framesCollector, TJointType joint) : base(framesCollector)
        {
            _joint = joint;
            MinimalVectorLength = 0.01f;
            MinimalPeriodBetweenGestures = 250;
        }

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            if (!Configuration.handNavigation) return;

            var frames = _framesCollector.GetFrames(LastFrameTimestamp).ToList();
            if (frames.Count == 0)
            {
                return;
            }

            var joints = frames.Select(a => a.GetNearestSkeleton().Joints.FirstOrDefault(b => b.JointType == _joint)).ToList();
            Vector3 calculateDisplacementVector = CalculateDisplacement(joints);

            if (calculateDisplacementVector.Length > MinimalVectorLength)
            {
                LogString.Log("Event: MovementAnalyzer: " + calculateDisplacementVector.X + " " + calculateDisplacementVector.Y + " " + calculateDisplacementVector.Z);
                var duration = CalculateDuration(frames);
                Raise(()=>RaiseEvent(frames, calculateDisplacementVector, duration));
            }
        }

        protected void RaiseEvent(List<Frame> frames, Vector3 calculateDisplacementVector, long duration)
        {
            if (frames.Last().Timestamp - LastFrameTimestamp > MinimalPeriodBetweenGestures && MovementDetected != null)
            {
                MovementDetected(this, new MovementEventArgs(calculateDisplacementVector, duration, _joint));
            }
        }

        protected static long CalculateDuration(List<Frame> frames)
        {
            var duration = frames.Last().Timestamp - frames.First().Timestamp;
            return duration;
        }

        protected static Vector3 CalculateDisplacement(List<TJoint> joints)
        {
            return joints.Last().Position.ToVector3() - joints.First().Position.ToVector3();
        }
    }
}
