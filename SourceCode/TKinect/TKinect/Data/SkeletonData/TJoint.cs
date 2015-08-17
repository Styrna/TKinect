using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace TKinect.Data.SkeletonData
{
    public class TJoint
    {
        public TJointType JointType { get; set; }
        public TSkeletonPoint Position { get; set; }
        public TJointTrackingState TrackingState { get; set; }

        public TJoint() {}
        public TJoint(Joint joint)
        {
            JointType = (TJointType)joint.JointType;
            Position = new TSkeletonPoint(joint.Position);
            TrackingState = (TJointTrackingState) joint.TrackingState;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((int)JointType);
            Position.Write(writer);
            writer.Write((int)TrackingState);
        }

        public void Read(BinaryReader reader)
        {
            JointType = (TJointType)reader.ReadInt32();
            Position = new TSkeletonPoint();
            Position.Read(reader);
            TrackingState = (TJointTrackingState)reader.ReadInt32();
        }
    }
}
