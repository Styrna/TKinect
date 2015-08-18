using System;
using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using Console;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class PostureAnalyzer : FramesAnalyzer
    {
        public delegate void PostureDetectedHandler(object sender, PostureEventArgs args);
        public event PostureDetectedHandler PostureDetected;

        protected float MaxRange { get; set; }
        protected float Epsilon { get; set; }

        private readonly int _postureFramesDuration;
        private PosturesEnum? _lastPosture;

        public PostureAnalyzer(FramesCollector framesCollector, int postureFramesDuration = 10) : base(framesCollector)
        {
            _postureFramesDuration = postureFramesDuration;
            Epsilon = 0.2f;
            MaxRange = 0.25f;
        }


        int handsJoined = 0;
        int leftHandOverHead = 0;
        int leftHello = 0;
        int rightHandOverHead = 0;
        int rightHello = 0;

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            if (!Configuration.postureNavigation) return;

            var frames =_framesCollector.GetFrames(LastFrameTimestamp).ToList();
            if (frames.Count == 0)
            {
                return;
            }

            Vector3? headPosition = null;
            Vector3? leftHandPosition = null;
            Vector3? rightHandPosition = null;
            
            

            var skeleton = frames.Last().GetNearestSkeleton();
            //foreach (Skeleton skeleton in frames.Select(a=>a.GetNearestSkeleton()))
            //{
            foreach (var joint in skeleton.Joints)
            {
                if (joint.TrackingState != TJointTrackingState.Tracked)
                    continue;

                switch (joint.JointType)
                {
                    case TJointType.Head:
                        headPosition = joint.Position.ToVector3();
                        break;
                    case TJointType.HandLeft:
                        leftHandPosition = joint.Position.ToVector3();
                        break;
                    case TJointType.HandRight:
                        rightHandPosition = joint.Position.ToVector3();
                        break;
                }
            }

            // HandsJoined
            if (CheckHandsJoined(rightHandPosition, leftHandPosition, headPosition))
            {
                handsJoined++;
                if (handsJoined == _postureFramesDuration)
                {
                    handsJoined = 0;
                    LogString.Log("Event: HandsJoined");
                    RaisePostureDetected(PosturesEnum.HandsJoined);
                }
            }

            // LeftHandOverHead
            if (CheckHandOverHead(headPosition, leftHandPosition))
            {
                leftHandOverHead++;
                if (leftHandOverHead == _postureFramesDuration)
                {
                    leftHandOverHead = 0;
                    LogString.Log("Event: LeftHandOverHead");
                    RaisePostureDetected(PosturesEnum.LeftHandOverHead);
                }
            }

            // RightHandOverHead
            if (CheckHandOverHead(headPosition, rightHandPosition))
            {
                rightHandOverHead++;
                if (rightHandOverHead == _postureFramesDuration)
                {
                    rightHandOverHead = 0;
                    LogString.Log("Event: RightHandOverHead");
                    RaisePostureDetected(PosturesEnum.RightHandOverHead);
                }
            }

            // LeftHello
            if (CheckHello(headPosition, leftHandPosition))
            {
                leftHello++;
                if (leftHello == _postureFramesDuration)
                {
                    leftHello = 0;
                    LogString.Log("Event: LeftHello");
                    RaisePostureDetected(PosturesEnum.LeftHello);
                }
            }

            // RightHello
            if (CheckHello(headPosition, rightHandPosition))
            {
                rightHello++;
                if (rightHello == _postureFramesDuration)
                {
                    rightHello = 0;
                    LogString.Log("Event: RightHello");
                    RaisePostureDetected(PosturesEnum.RightHello);
                }
            }
            //}
            _lastPosture = null;
        }

        private void RaisePostureDetected(PosturesEnum posture)
        {
            if (_lastPosture == posture)
            {
                return;
            }
            Raise(() => PostureDetected(this, new PostureEventArgs { Posture = posture }));
            _lastPosture = posture;
        }

        bool CheckHandOverHead(Vector3? headPosition, Vector3? handPosition)
        {
            if (!handPosition.HasValue || !headPosition.HasValue)
                return false;

            if (handPosition.Value.Y < headPosition.Value.Y)
                return false;

            if (Math.Abs(handPosition.Value.X - headPosition.Value.X) > MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Z - headPosition.Value.Z) > MaxRange)
                return false;

            return true;
        }


        bool CheckHello(Vector3? headPosition, Vector3? handPosition)
        {
            if (!handPosition.HasValue || !headPosition.HasValue)
                return false;

            if (Math.Abs(handPosition.Value.X - headPosition.Value.X) < MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Y - headPosition.Value.Y) > MaxRange)
                return false;

            if (Math.Abs(handPosition.Value.Z - headPosition.Value.Z) > MaxRange)
                return false;

            return true;
        }

        bool CheckHandsJoined(Vector3? leftHandPosition, Vector3? rightHandPosition, Vector3? headPosition)
        {
            if (!leftHandPosition.HasValue || !rightHandPosition.HasValue || !headPosition.HasValue)
                return false;

            float distance = (leftHandPosition.Value - rightHandPosition.Value).Length;
            float distanceFromHead = Math.Abs((leftHandPosition.Value.Z + rightHandPosition.Value.Z) / 2 - headPosition.Value.Z);

             
            if (distance > Epsilon || distanceFromHead > 0.4)
            {

                return false;
            }
            return true;
        }    
        
    }
}
