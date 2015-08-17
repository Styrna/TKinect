#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     This class contains the skeleton frame returned by the Kinect API.
    public sealed class SkeletonFrame : IDisposable
    {
        // Summary:
        //     Gets the depth filter that was applied to the frame.
        public IDepthFilter DepthFilter { get; }
        //
        // Summary:
        //     Gets or sets the floor's clip plane.  The floats should be in this order
        //     from the native type: x, y, z, w.
        public Tuple<float, float, float, float> FloorClipPlane { get; set; }
        //
        // Summary:
        //     Gets or sets the frame number for the frame.
        public int FrameNumber { get; set; }
        //
        // Summary:
        //     Gets the total length of the skeleton data buffer of this SkeletonFrame.
        public int SkeletonArrayLength { get; }
        //
        // Summary:
        //     Gets or sets the timestamp for the frame.
        public long Timestamp { get; set; }
        //
        // Summary:
        //     Gets the tracking mode in which this frame was captured.
        [Obsolete("SkeletonFrame.TrackingMode property is reserved for future use.  Do not use this property; this value may change in a future release.", false)]
        public SkeletonTrackingMode TrackingMode { get; }

        // Summary:
        //     This method copies the skeleton data to an array of skeleton data.
        //
        // Parameters:
        //   skeletonData:
        //     The target array to receive the data.
        public void CopySkeletonDataTo(Skeleton[] skeletonData);
        //
        // Summary:
        //     This method disposes the skeleton frame.
        public void Dispose();
    }
}
