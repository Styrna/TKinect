#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     Arguments for the ColorFrameReady events.
    public sealed class ColorImageFrameReadyEventArgs : EventArgs
    {
        // Summary:
        //     Container for one frame's worth of color sensor image data Can return null
        //     if the data is not available.  Upon success, returns the ColorImageFrame
        //     corresponding to this event, which must be Disposed.
        //
        // Returns:
        //     A new ColorImageFrame.
        public ColorImageFrame OpenColorImageFrame();
    }
}
