using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using Console;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class ContinousMovementAnalyzer : MovementAnalyzer
    {
        private Vector3 _oldDisplacementVector;

        public ContinousMovementAnalyzer(FramesCollector framesCollector, TJointType joint) : base(framesCollector, joint)
        {
        }

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            if (!Configuration.movementNavigation) return;
            var frames = _framesCollector.GetFrames().ToList();
            if (frames.Count == 0)
            {
                return;
            }

            var joints = frames.Select(a => a.GetNearestSkeleton().Joints.First(j => j.JointType ==_joint)).ToList();
            Vector3 calculateDisplacementVector = CalculateDisplacement(joints);

            if (calculateDisplacementVector.Length > MinimalVectorLength)
            {
                _oldDisplacementVector = calculateDisplacementVector;
            }
            var duration = CalculateDuration(frames);

            LogString.Log("Event: ContinousMovementAnalyzer: " + calculateDisplacementVector.X + " " + calculateDisplacementVector.Y + " " + calculateDisplacementVector.Z);
            Raise(()=>RaiseEvent(frames, _oldDisplacementVector, duration));
        }
    }
}
