#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     The event arguments used when a skeletion frame is ready.
    public sealed class SkeletonFrameReadyEventArgs : EventArgs
    {
        // Summary:
        //     Container for one frame's worth of skeleton sensor image data Can return
        //     null if the data is not available.  Upon success, returns the SkeletonFrame
        //     corresponding to this event, which must be Disposed.
        //
        // Returns:
        //     A new SkeletonFrame.
        public SkeletonFrame OpenSkeletonFrame();
    }
}
