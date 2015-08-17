#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     A frame used specifically for depth images. It provides access to the dimensions,
    //     format and pixel data for a depth frame, and allows for mapping of coordinates
    //     between skeleton frames and color frames.
    public sealed class DepthImageFrame : ImageFrame
    {
        // Summary:
        //     A bitmask for extracting the player index bit field from the depth value.
        public const int PlayerIndexBitmask = 7;
        //
        // Summary:
        //     The width of the player index bitmask.
        public const int PlayerIndexBitmaskWidth = 3;

        // Summary:
        //     Gets the depth filter that was applied to the frame.
        public IDepthFilter DepthFilter { get; }
        //
        // Summary:
        //     Gets this frame's Framerate and Resolution.
        public DepthImageFormat Format { get; }
        //
        // Summary:
        //     Gets the maximum reliable Depth value in mm for the depth sensor range setting
        //     used to capture this frame.
        public int MaxDepth { get; }
        //
        // Summary:
        //     Gets the minimum reliable Depth value in mm for the depth sensor range setting
        //     used to capture this frame.
        public int MinDepth { get; }
        //
        // Summary:
        //     Total length of the pixel data buffer of this ImageFrame.
        public override int PixelDataLength { get; }
        //
        // Summary:
        //     Gets the depth sensor range with which this frame was captured.
        public DepthRange Range { get; }

        // Summary:
        //     This method copies the frame's pixel data to a pre-allocated pixel array.
        //
        // Parameters:
        //   pixelData:
        //     The pixel array to receive the data.  It must be exactly PixelDataLength
        //     in length.
        public void CopyDepthImagePixelDataTo(DepthImagePixel[] pixelData);
        //
        // Summary:
        //     This method copies the frame's DepthImagePixel data to a pre-allocated byte
        //     array.
        //
        // Parameters:
        //   pixelData:
        //     The IntPtr of the byte array.
        //
        //   pixelDataLength:
        //     The count of DepthImagePixels to copy to pixelData. This must be equal to
        //     the frame’s PixelDataLength.
        public void CopyDepthImagePixelDataTo(IntPtr pixelData, int pixelDataLength);
        //
        // Summary:
        //     This method copies the frame's pixel data to a pre-allocated pixel array.
        //
        // Parameters:
        //   pixelData:
        //     The pixel array to receive the data.  It must be exactly PixelDataLength
        //     in length.
        public void CopyPixelDataTo(short[] pixelData);
        //
        // Summary:
        //     This method copies the frame's pixel data to a pre-allocated byte array.
        //
        // Parameters:
        //   pixelData:
        //     The IntPtr of the byte array.
        //
        //   pixelDataLength:
        //     The count of Int16s to copy to pixelData. This must be equal to the frame’s
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
        //     This method provides the DepthImagePixel array that contains the depth image's
        //     pixel data.
        //
        // Returns:
        //     A DepthImagePixel array of the depth frame's pixel data.
        public DepthImagePixel[] GetRawPixelData();
        //
        // Summary:
        //     Looks up the depth frame coordinates for a given skeleton point.
        //
        // Parameters:
        //   skeletonPoint:
        //     The supplied skeleton point.
        //
        // Returns:
        //     The ImagePoint that contains the X, Y and depth value of the given skeleton
        //     point.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapSkeletonPointToDepthPoint", false)]
        public DepthImagePoint MapFromSkeletonPoint(SkeletonPoint skeletonPoint);
        //
        // Summary:
        //     This maps a depth coordinate to a color coordinate.
        //
        // Parameters:
        //   depthX:
        //     The X coordinate of the depth frame.
        //
        //   depthY:
        //     The Y coordinate of the depth frame.
        //
        //   colorImageFormat:
        //     The color format being used.
        //
        // Returns:
        //     An ImagePoint that contains the X, Y locations in the color frame.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapDepthPointToColorPoint", false)]
        public ColorImagePoint MapToColorImagePoint(int depthX, int depthY, ColorImageFormat colorImageFormat);
        //
        // Summary:
        //     Looks up the skeleton point location of the given depth X, Y.
        //
        // Parameters:
        //   depthX:
        //     The X coordinate of the depth frame.
        //
        //   depthY:
        //     The Y coordinate of the depth frame.
        //
        // Returns:
        //     The skeleton point for the given X, Y.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapDepthPointToSkeletonPoint", false)]
        public SkeletonPoint MapToSkeletonPoint(int depthX, int depthY);
    }
}
