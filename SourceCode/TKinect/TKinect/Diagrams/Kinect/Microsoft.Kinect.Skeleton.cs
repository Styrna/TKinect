#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     This class is used to track all of the known data about a skeleton returned
    //     from the skeleton stream.
    [Serializable]
    public class Skeleton
    {
        // Summary:
        //     Initializes a new instance of the Skeleton class with a default JointCollection.
        public Skeleton();

        // Summary:
        //     Gets or sets the skeleton's bone orientations.
        public BoneOrientationCollection BoneOrientations { get; protected set; }
        //
        // Summary:
        //     Gets or sets the edges that this skeleton is clipped on.
        public FrameEdges ClippedEdges { get; set; }
        //
        // Summary:
        //     Gets or sets the skeleton's joints.
        public JointCollection Joints { get; protected set; }
        //
        // Summary:
        //     Gets or sets the skeleton's position.
        public SkeletonPoint Position { get; set; }
        //
        // Summary:
        //     Gets or sets the skeleton's tracking ID.
        public int TrackingId { get; set; }
        //
        // Summary:
        //     Gets or sets the skeleton's current tracking state.
        public SkeletonTrackingState TrackingState { get; set; }
    }
}
