#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     Image formats for the Color Image Stream. It represents Image Data format,
    //     Framerate, and Resolution.
    public enum ColorImageFormat
    {
        // Summary:
        //     The image format is undefined.
        Undefined = 0,
        //
        // Summary:
        //     RGB data (32 bits per pixel, layout corresponding to PixelFormats.Bgr32).
        //      Resolution of 640 by 480 at 30 Frames per second.
        RgbResolution640x480Fps30 = 1,
        //
        // Summary:
        //     RGB data (32 bits per pixel, layout corresponding to PixelFormats.Bgr32).
        //      Resolution of 1280 by 960 at 12 Frames per second.
        RgbResolution1280x960Fps12 = 2,
        //
        // Summary:
        //     The data is collected from the Kinect as YUV, but it is translated to: RGB
        //     data (32 bits per pixel, layout corresponding to PixelFormats.Bgr32).  Resolution
        //     of 640 by 480 at 15 Frames per second.
        YuvResolution640x480Fps15 = 3,
        //
        // Summary:
        //     YUV data (32 bits per pixel, layout corresponding to D3DFMT_LIN_UYVY).  Resolution
        //     of 640 by 480 at 15 Frames per second.
        RawYuvResolution640x480Fps15 = 4,
        //
        // Summary:
        //     Infrared data (16 bits per pxel) Resolution of 640 by 480 at 30 Frames per
        //     second.
        InfraredResolution640x480Fps30 = 5,
        //
        // Summary:
        //     Bayer data (8 bits per pixel, layout in alternating pixels of red, green
        //     and blue).  Resolution of 640 by 480 at 30 Frames per second.
        RawBayerResolution640x480Fps30 = 6,
        //
        // Summary:
        //     Bayer data (8 bits per pixel, layout in alternating pixels of red, green
        //     and blue).  Resolution of 1280 by 960 at 12 Frames per second.
        RawBayerResolution1280x960Fps12 = 7,
    }
}
