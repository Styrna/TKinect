using System.Collections.Generic;
using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using Console;
using Kinect.Toolbox.Utils;
using System;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class HandInFrontAnalyzer : FramesAnalyzer
    {
        private float ZoomSum = 0;

        private TSkeletonPoint LeftLastPosition, RightLastPosition;
        private float LastHandsDistance = 0;
        private int ignoredCalls = Configuration.framesSkipped;
        private bool ignoreCallsTrigger = false;

        private bool leftHandActivated = false, rightHandActivated = false, bothHandsActivated = false;

        public delegate void MovementDetectedHandler(object sender, MovementEventArgs args);
        public event MovementDetectedHandler MovementDetected;
        public int MinimalPeriodBetweenGestures { get; set; }

        public HandInFrontAnalyzer(FramesCollector framesCollector) : base(framesCollector)
        {

            MinimalPeriodBetweenGestures = Configuration.minimalPeriodBetweenGestures;

            CallsSkipp = Configuration.callsSkipped;
            CallCounter = CallsSkipp;
        }

        private int CallsSkipp;
        private int CallCounter;

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            var frames = _framesCollector.GetFrames(LastFrameTimestamp).ToList();
            if (frames.Count == 0)
            {
                return;
            }

            var torsoPosHistory = frames.Select(a => a.GetNearestSkeleton().Joints.FirstOrDefault(b => b.JointType == TJointType.Spine)).ToList();
            var LeftHandPosHistory = frames.Select(a => a.GetNearestSkeleton().Joints.FirstOrDefault(b => b.JointType == TJointType.HandLeft)).ToList();
            var RightHandPosHistory = frames.Select(a => a.GetNearestSkeleton().Joints.FirstOrDefault(b => b.JointType == TJointType.HandRight)).ToList();

            var lastLeftHandFrame = LeftHandPosHistory.Last();
            var lastRightHandFrame = RightHandPosHistory.Last();
            var lastTorsoFrame = torsoPosHistory.Last();

            var diffLeft = lastTorsoFrame.Position.Z - lastLeftHandFrame.Position.Z;
            var diffRight = lastTorsoFrame.Position.Z - lastRightHandFrame.Position.Z;

            if (ignoredCalls > 0)
            {
                ignoreCallsTrigger = true;
                ignoredCalls--;
                return;
            }
            else if (ignoreCallsTrigger == true)
            {
                ignoreCallsTrigger = false;
                LeftLastPosition = lastLeftHandFrame.Position;
                RightLastPosition = lastRightHandFrame.Position;
                LastHandsDistance = CalculateDistance(lastLeftHandFrame, lastRightHandFrame);
            }

            Calibrate(diffLeft, diffRight);

            bool left = LeftHandActive(diffLeft, lastLeftHandFrame.Position);
            bool right = RightHandActive(diffRight, lastRightHandFrame.Position);

            if (left && right)
            {
                if (!bothHandsActivated)
                {
                    bothHandsActivated = true;
                    LastHandsDistance = CalculateDistance(lastLeftHandFrame, lastRightHandFrame);
                    LogString.Log("BothHands Activated");
                }
                
                NavigateByBothHands(lastLeftHandFrame, lastRightHandFrame, frames);
            }
            else if (left)
            {
                NavigateByLeftHand(lastLeftHandFrame, frames);
            }
            else if (right)
            {
                NavigateByRightHand(lastRightHandFrame, frames);
            }
        }

        private void NavigateByRightHand(TJoint lastRightHandFrame, List<Frame> frames)
        {
            var transitionX = lastRightHandFrame.Position.X - RightLastPosition.X;
            var transitionY = lastRightHandFrame.Position.Y - RightLastPosition.Y;

            var transitionVector = new Vector3() { X = -transitionX, Y = -transitionY };

            Raise(() => RaiseEvent(frames, transitionVector, 0, ControlType.RIGHT_HAND));

            RightLastPosition = lastRightHandFrame.Position;
        }

        private void NavigateByBothHands(TJoint lastLeftHandFrame, TJoint lastRightHandFrame, List<Frame> frames)
        {
            var distance = CalculateDistance(lastLeftHandFrame, lastRightHandFrame);

            ZoomSum += distance - LastHandsDistance;

            if (ZoomSum > Configuration.minimalHandDistanceChanged)
            {
                var transitionVector = new Vector3() { X = ZoomSum };

                //LogString.Log("Hand: " + Kinect.Toolbox.Utils.Extensions.Print(jointPos.Position));
                Raise(() => RaiseEvent(frames, transitionVector, -1, ControlType.BOTH_HANDS));
                ZoomSum = 0;
                
            }
            else if (ZoomSum < -Configuration.minimalHandDistanceChanged)
            {
                var transitionVector = new Vector3() { X = ZoomSum };

                //LogString.Log("Hand: " + Kinect.Toolbox.Utils.Extensions.Print(jointPos.Position));
                Raise(() => RaiseEvent(frames, transitionVector, -1, ControlType.BOTH_HANDS));

                ZoomSum = 0;
            }

            LastHandsDistance = distance;
        }

        private float CalculateDistance(TJoint lastLeftHandFrame, TJoint lastRightHandFrame)
        {
            var transitionLeftX = lastLeftHandFrame.Position.X - LeftLastPosition.X;
            var transitionLeftY = lastLeftHandFrame.Position.Y - LeftLastPosition.Y;

            var transitionRightX = lastRightHandFrame.Position.X - RightLastPosition.X;
            var transitionRightY = lastRightHandFrame.Position.Y - RightLastPosition.Y;

            var distance = (float) Math.Sqrt((transitionRightX - transitionLeftX) * (transitionRightX - transitionLeftX) + (transitionRightY - transitionLeftY) * (transitionRightY - transitionLeftY));

            return distance;
        }
        
        private void NavigateByLeftHand(TJoint lastLeftHandFrame, List<Frame> frames)
        {
            var transitionX = lastLeftHandFrame.Position.X - LeftLastPosition.X;
            var transitionY = lastLeftHandFrame.Position.Y - LeftLastPosition.Y;

            var transitionVector = new Vector3() { X = transitionX, Y = transitionY };

            Raise(() => RaiseEvent(frames, transitionVector, 0, ControlType.LEFT_HAND));

            LeftLastPosition = lastLeftHandFrame.Position;

        }

        private bool RightHandActive(double diffRight, TSkeletonPoint lastRightHandFrame)
        {
            if (diffRight > Configuration.activeHandDistance)
            {
                if ( ! rightHandActivated)
                {
                    LogString.Log("RightHand Activated");
                    RightLastPosition = lastRightHandFrame;
                    rightHandActivated = true;
                    ignoredCalls = Configuration.framesSkipped;
                }

                return true;
            }
            else if (rightHandActivated)
            {
                LogString.Log("RightHand Deactivated");
                bothHandsActivated = false;
                rightHandActivated = false;
            }
            return false;
        }

        private bool LeftHandActive(double diffLeft, TSkeletonPoint lastLeftHandFrame)
        {
            if (diffLeft > Configuration.activeHandDistance)
            {
                if (!leftHandActivated)
                {
                    LogString.Log("LeftHand Activated");
                    LeftLastPosition = lastLeftHandFrame;
                    leftHandActivated = true;
                    ignoredCalls = Configuration.framesSkipped;
                }

                return true;
            }
            else if (leftHandActivated)
            {
                LogString.Log("LeftHand Deactivated");
                bothHandsActivated = false;
                leftHandActivated = false;
            }
            return false;
        }

        private void Calibrate(double distance, double distance2)
        {
            if (distance < distance2) distance = distance2;
            if (distance > Configuration.maxHandLength)
            {
                Configuration.maxHandLength = distance;
                Configuration.activeHandDistance = Configuration.maxHandLength * Configuration.activePart;
            };
        }

        protected void RaiseEvent(List<Frame> frames, Vector3 calculateDisplacementVector, long duration, ControlType controlType)
        {
            if (frames.Last().Timestamp - LastFrameTimestamp > MinimalPeriodBetweenGestures && MovementDetected != null)
            {
                MovementDetected(this, new MovementEventArgs(calculateDisplacementVector, duration, TJointType.HandLeft, controlType));
            }
        }
    }
}
