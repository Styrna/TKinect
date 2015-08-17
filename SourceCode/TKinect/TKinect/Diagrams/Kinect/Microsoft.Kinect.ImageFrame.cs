#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     A container for per-frame sensor data buffers.
    public abstract class ImageFrame : IDisposable
    {
        // Summary:
        //     Gets the bytes per pixel of this ImageFrame.
        public int BytesPerPixel { get; }
        //
        // Summary:
        //     Gets the frame identification.
        public int FrameNumber { get; }
        //
        // Summary:
        //     Gets the height in pixels of this ImageFrame.
        public int Height { get; }
        //
        // Summary:
        //     Gets the total length of the pixel data buffer of this ImageFrame.
        public abstract int PixelDataLength { get; }
        //
        // Summary:
        //     Gets the time of this ImageFrame.
        public long Timestamp { get; }
        //
        // Summary:
        //     Gets the width in pixels of this ImageFrame.
        public int Width { get; }

        // Summary:
        //     Disposes the image frame.
        public void Dispose();
        //
        // Summary:
        //     To be implemented by derived classes to return data to free lists.
        //
        // Parameters:
        //   disposing:
        //     Specify true to indicate that the class should clean up all resources.
        protected abstract void Dispose(bool disposing);
    }
}
