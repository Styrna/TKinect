#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;
using System.Diagnostics;

namespace Microsoft.Kinect
{
    // Summary:
    //     This struct is used to describe a skeleton's joint.
    [Serializable]
    [DebuggerDisplay("Position:{Position} JointType:{JointType} TrackingState:{TrackingState}")]
    public struct Joint
    {

        // Summary:
        //     This method compares two joint objects.
        //
        // Parameters:
        //   joint1:
        //     The first joint to compare.
        //
        //   joint2:
        //     The second joint to compare.
        //
        // Returns:
        //     It returns true if they are not equal and false otherwise.
        public static bool operator !=(Joint joint1, Joint joint2);
        //
        // Summary:
        //     This method compares two joint objects.
        //
        // Parameters:
        //   joint1:
        //     The first joint to compare.
        //
        //   joint2:
        //     The second joint to compare.
        //
        // Returns:
        //     It returns true if they are equal and false otherwise.
        public static bool operator ==(Joint joint1, Joint joint2);

        // Summary:
        //     Gets the joint's type.
        public JointType JointType { get; internal set; }
        //
        // Summary:
        //     Gets or sets the joint's position.
        public SkeletonPoint Position { get; set; }
        //
        // Summary:
        //     Gets or sets the tracking state of this joint.
        public JointTrackingState TrackingState { get; set; }

        // Summary:
        //     This method compares two joint objects.
        //
        // Parameters:
        //   joint:
        //     The joint to compare.
        //
        // Returns:
        //     It returns true if they are equal and false otherwise.
        public bool Equals(Joint joint);
        //
        // Summary:
        //     This method compares two joint objects.
        //
        // Parameters:
        //   obj:
        //     The object to compare.
        //
        // Returns:
        //     It returns true if they are equal and false otherwise.
        public override bool Equals(object obj);
        //
        // Summary:
        //     This method calculates the hash code for a joint.
        //
        // Returns:
        //     The joint's hash code.
        public override int GetHashCode();
    }
}
