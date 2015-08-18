using System;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class MovementEventArgs : EventArgs
    {
        public Vector3 Displacement { get; set; }
        public long Duration { get; set; }
        public TJointType JointType { get; set; }

        public ControlType ControlType { get; set; }

        public MovementEventArgs(Vector3 displacement, long duration, TJointType jointType)
        {
            Displacement = displacement;
            Duration = duration;
            JointType = jointType;
        }

        public MovementEventArgs(Vector3 displacement, long duration, TJointType jointType, ControlType controlType)
        {
            Displacement = displacement;
            Duration = duration;
            JointType = jointType;
            ControlType = controlType;
        }
    }

    public enum ControlType
    {
        LEFT_HAND,
        RIGHT_HAND,
        BOTH_HANDS
    }
}