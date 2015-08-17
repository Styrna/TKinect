using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace TKinect.Data.SkeletonData
{
    public class TSkeleton
    {
        public TFrameEdges ClippedEdges { get; set; }
        public TJoint[] Joints { get; set; }
        public TSkeletonPoint Position { get; set; }
        public int TrackingId { get; set; }
        public TSkeletonTrackingState TrackingState { get; set; }

        public TSkeleton() {  }
        public TSkeleton(Skeleton skeleton)
        {
            ClippedEdges = (TFrameEdges) skeleton.ClippedEdges;
            Joints = new TJoint[skeleton.Joints.Count];
            int i = 0;
            foreach (var joint in skeleton.Joints)
            {
                Joints[i] = new TJoint((Joint)joint);
                i++;
            }
            Position = new TSkeletonPoint(skeleton.Position);
            TrackingId = skeleton.TrackingId;
            TrackingState = (TSkeletonTrackingState)skeleton.TrackingState;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((int)ClippedEdges);
            writer.Write(Joints.Length);
            foreach (var joint in Joints)
                joint.Write(writer);
            Position.Write(writer);
            writer.Write(TrackingId);
            writer.Write((int) TrackingState);
        }

        public void Read(BinaryReader reader)
        {
            ClippedEdges = (TFrameEdges)reader.ReadInt32();
            int jointCount = reader.ReadInt32();
            Joints = new TJoint[jointCount];
            for (int jx = 0; jx < jointCount; jx++)
            {
                Joints[jx] = new TJoint();
                Joints[jx].Read(reader);
            }
            Position = new TSkeletonPoint();
            Position.Read(reader);
            TrackingId = reader.ReadInt32();
            TrackingState = (TSkeletonTrackingState)reader.ReadInt32();
        }
    }
}
