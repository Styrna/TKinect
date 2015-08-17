using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TKinect.Data.DepthData;
using TKinect.Data.SkeletonData;

namespace TKinectTests.Data
{
    [TestFixture]
    public class SkeletonFrameTests : TFrameTests<TSkeletonFrame>
    {
        protected override TSkeletonFrame InitializeFrame()
        {
            var skeletons = new TSkeleton[10];
            for (int i = 0; i < 10; i++)
            {
                skeletons[i] = GetSkeleton();
            }

            return new TSkeletonFrame()
            {
                Timestamp = Random.Next(),
                FrameNumber = Random.Next(),
                FloorClipPlane = new Tuple<float, float, float, float>(Random.NextFloat(), Random.NextFloat(), Random.NextFloat(), Random.NextFloat()),
                SkeletonArrayLength = 10,
                Skeletons = skeletons,
            };
        }

        private TSkeleton GetSkeleton()
        {
            var joints = new TJoint[10];
            for (int i = 0; i < 10; i++)
            {
                joints[i] = GetJoint();
            }

            return new TSkeleton()
            {
                ClippedEdges = TFrameEdges.Left,
                Position = GetSkeletonPoint(),
                TrackingId = new Random().Next(),
                TrackingState = TSkeletonTrackingState.Tracked,
                Joints = joints
            };
        }

        private TJoint GetJoint()
        {
            return new TJoint()
            {
                JointType = TJointType.ElbowLeft,
                Position = GetSkeletonPoint(),
                TrackingState = TJointTrackingState.Tracked
            };
        }

        private TSkeletonPoint GetSkeletonPoint()
        {
            return new TSkeletonPoint()
            {
                X = new Random().Next(),
                Y = new Random().Next(),
                Z = new Random().Next()
            };
        }

        protected override void CompareFrames(TSkeletonFrame frame1, TSkeletonFrame frame2)
        {
            Assert.True(frame1.SkeletonArrayLength == frame2.SkeletonArrayLength);
            Assert.True(frame1.Timestamp == frame2.Timestamp);
            Assert.True(frame1.FrameNumber == frame2.FrameNumber);

            Assert.True(frame1.FloorClipPlane.Item1 == frame2.FloorClipPlane.Item1);
            Assert.True(frame1.FloorClipPlane.Item2 == frame2.FloorClipPlane.Item2);
            Assert.True(frame1.FloorClipPlane.Item3 == frame2.FloorClipPlane.Item3);
            Assert.True(frame1.FloorClipPlane.Item4 == frame2.FloorClipPlane.Item4);

            for (int i = 0; i < 10; i++)
            {
                CompareSkeletons(frame1.Skeletons[i], frame2.Skeletons[i]);
            }
        }

        private void CompareSkeletons(TSkeleton skeleton1, TSkeleton skeleton2)
        {
            Assert.True(skeleton1.ClippedEdges == skeleton2.ClippedEdges);
            Assert.True(skeleton1.TrackingId == skeleton2.TrackingId);
            Assert.True(skeleton1.TrackingState == skeleton2.TrackingState);

            for (int i = 0; i < 10; i++)
            {
                CompareJoint(skeleton1.Joints[i], skeleton2.Joints[i]);
            }

            CompareSkeletonPoints(skeleton1.Position, skeleton2.Position);
        }

        private void CompareJoint(TJoint joint1, TJoint joint2)
        {
            Assert.True(joint1.JointType == joint2.JointType);

            CompareSkeletonPoints(joint1.Position, joint2.Position);

            Assert.True(joint1.TrackingState == joint2.TrackingState);
        }

        private void CompareSkeletonPoints(TSkeletonPoint skeletonP1, TSkeletonPoint skeletonP2)
        {
            Assert.True(skeletonP1.X == skeletonP2.X);
            Assert.True(skeletonP1.Y == skeletonP2.Y);
            Assert.True(skeletonP1.Z == skeletonP2.Z);
        }
    }
}