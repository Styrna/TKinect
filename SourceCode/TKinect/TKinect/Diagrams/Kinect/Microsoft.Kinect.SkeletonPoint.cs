#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;
using System.Diagnostics;

namespace Microsoft.Kinect
{
    // Summary:
    //     This struct describes a 3 dimensional point in skeleton space.
    [Serializable]
    [DebuggerDisplay("X:{X} Y:{Y} Z:{Z}")]
    public struct SkeletonPoint
    {

        // Summary:
        //     This method compares two skeleton point objects.
        //
        // Parameters:
        //   skeletonPoint1:
        //     The first SkeletonPoint to compare.
        //
        //   skeletonPoint2:
        //     The second SkeletonPoint to compare.
        //
        // Returns:
        //     It returns true if they are not equal and false otherwise.
        public static bool operator !=(SkeletonPoint skeletonPoint1, SkeletonPoint skeletonPoint2);
        //
        // Summary:
        //     This method compares two skeleton point objects.
        //
        // Parameters:
        //   skeletonPoint1:
        //     The first SkeletonPoint to compare.
        //
        //   skeletonPoint2:
        //     The second SkeletonPoint to compare.
        //
        // Returns:
        //     It returns true if they are equal and false otherwise.
        public static bool operator ==(SkeletonPoint skeletonPoint1, SkeletonPoint skeletonPoint2);

        // Summary:
        //     Gets or sets the X coordinate of the skeleton point.
        public float X { get; set; }
        //
        // Summary:
        //     Gets or sets the Y coordinate of the skeleton point.
        public float Y { get; set; }
        //
        // Summary:
        //     Gets or sets the Z coordinate of the skeleton point.
        public float Z { get; set; }

        // Summary:
        //     This method compares two skeleton point objects.
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
        //     This method compares two skeleton point objects.
        //
        // Parameters:
        //   skeletonPoint:
        //     The SkeletonPoint to compare.
        //
        // Returns:
        //     It returns true if they are equal and false otherwise.
        public bool Equals(SkeletonPoint skeletonPoint);
        //
        // Summary:
        //     This gets the hash code for a given skeleton point.
        //
        // Returns:
        //     The calculated hash code.
        public override int GetHashCode();
    }
}
