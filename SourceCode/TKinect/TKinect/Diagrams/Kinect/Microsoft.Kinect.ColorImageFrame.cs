#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     A frame used specifically for color images.  It provides access to the dimensions,
    //     format and pixel data for a color frame.
    public sealed class ColorImageFrame : ImageFrame
    {
        // Summary:
        //     Gets this frame's Framerate and Resolution.
        public ColorImageFormat Format { get; }
        //
        // Summary:
        //     Gets the total length of the pixel data buffer of this ImageFrame.
        public override int PixelDataLength { get; }

        // Summary:
        //     This method copies the frame's pixel data to a pre-allocated byte array.
        //
        // Parameters:
        //   pixelData:
        //     The byte array to receive the data.  It must be exactly PixelDataLength in
        //     length.
        public void CopyPixelDataTo(byte[] pixelData);
        //
        // Summary:
        //     This method copies the frame's pixel data to a pre-allocated byte array.
        //
        // Parameters:
        //   pixelData:
        //     The IntPtr of the byte array.
        //
        //   pixelDataLength:
        //     The count of Bytes to copy to pixelData. This must be equal to the frame’s
        //     PixelDataLength.
        public void CopyPixelDataTo(IntPtr pixelData, int pixelDataLength);
        //
        // Summary:
        //     Disposes the frame object.
        //
        // Parameters:
        //   disposing:
        //     Specify true to indicate that the class should clean up all resources.
        protected override void Dispose(bool disposing);
        //
        // Summary:
        //     This method provides the raw array that contains the color frame's pixel
        //     data.
        //
        // Returns:
        //     A byte array of the color frame's pixel data.
        public byte[] GetRawPixelData();
    }
}
