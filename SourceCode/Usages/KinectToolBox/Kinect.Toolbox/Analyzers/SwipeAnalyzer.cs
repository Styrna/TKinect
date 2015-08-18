using System;
using System.Collections.Generic;
using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using Console;
using TKinect.Data.SkeletonData;
using TKinect.Data;

namespace Kinect.Toolbox.Analyzers
{
    public class SwipeAnalyzer : FramesAnalyzer
    {
        private readonly TJointType _joint;

        public float SwipeMinimalLength { get; set; }
        public float SwipeMaximalHeight { get; set; }
        public int SwipeMininalDuration { get; set; }
        public int SwipeMaximalDuration { get; set; }
        public int MinimalPeriodBetweenGestures { get; set; }

        public delegate void SwipeDetectedHandler(object sender, SwipeEventArgs args);
        public event SwipeDetectedHandler SwipeDetected;

        public SwipeAnalyzer(FramesCollector framesCollector, TJointType jointType) : base(framesCollector)
        {
            _joint = jointType;
            SwipeMinimalLength = 0.4f;
            SwipeMaximalHeight = 0.2f;
            SwipeMininalDuration = 250;
            SwipeMaximalDuration = 1500;
        }

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            if (!Configuration.swipeNavigation) return;

            var frames = _framesCollector.GetFrames(LastFrameTimestamp).ToList();
            if (frames.Count == 0)
            {
                return;
            }

            if (!EnsureDuration(frames))
            {
                return;
            }

            var joints = frames.Select(a => a.GetNearestSkeleton().Joints.First(j => j.JointType ==_joint)).ToList();

            // Swipe to right
            if (ScanPositions(joints, (p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight, // Height
                (p1, p2) => p2.X - p1.X > -0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength))//Length
            {
                LogString.Log("Event: SwipeToRight");
                Raise(()=>RaiseGestureDetected(new SwipeEventArgs(GesturesEnum.SwipeToRight)));
                return;
            }

            // Swipe to left
            if (ScanPositions(joints, (p1, p2) => Math.Abs(p2.Y - p1.Y) < SwipeMaximalHeight,  // Height
                (p1, p2) => p2.X - p1.X < 0.01f, // Progression to right
                (p1, p2) => Math.Abs(p2.X - p1.X) > SwipeMinimalLength))// Length
            {
                LogString.Log("Event: SwipeToLeft");
                Raise(()=>RaiseGestureDetected(new SwipeEventArgs(GesturesEnum.SwipeToLeft)));
                return;
            }
        }

        private void RaiseGestureDetected(SwipeEventArgs eventArgs)
        {
            if (DateTime.Now.Ticks - LastFrameTimestamp > MinimalPeriodBetweenGestures)
            {
                if (SwipeDetected != null)
                    SwipeDetected(this, eventArgs);
                }
        }

        private bool ScanPositions(List<TJoint> joints, Func<TSkeletonPoint, TSkeletonPoint, bool> heightFunction, Func<TSkeletonPoint, TSkeletonPoint, bool> directionFunction,
            Func<TSkeletonPoint, TSkeletonPoint, bool> lengthFunction)
        {
            int start = 0;

            for (int index = 1; index < joints.Count - 1; index++)
            {
                if (!heightFunction(joints[0].Position, joints[index].Position) || !directionFunction(joints[index].Position, joints[index + 1].Position))
                {
                    start = index;
                }

                if (lengthFunction(joints[index].Position, joints[start].Position))
                {
                    return true;
                }
            }

            return false;
        }

        private bool EnsureDuration(List<Frame> frames)
        {
            var duration = frames.Last().Timestamp - frames.First().Timestamp;
            return duration <= SwipeMaximalDuration && duration >= SwipeMininalDuration;
        }
    }

    public class SwipeEventArgs : EventArgs
    {
        public GesturesEnum Gesture { get; private set; }

        public SwipeEventArgs(GesturesEnum gesture)
        {
            Gesture = gesture;
        }
    }
}
