#region Assembly Microsoft.Kinect.dll, v1.8.0.0
// C:\Program Files\Microsoft SDKs\Kinect\v1.8\Assemblies\Microsoft.Kinect.dll
#endregion

using Microsoft.Kinect.Interop;
using System;

namespace Microsoft.Kinect
{
    // Summary:
    //     A Microsoft Kinect™ sensor.
    public sealed class KinectSensor : IDisposable
    {
        // Summary:
        //     Gets the audio source for the sensor.
        public KinectAudioSource AudioSource { get; }
        //
        // Summary:
        //     Gets the color stream for the sensor.
        public ColorImageStream ColorStream { get; }
        //
        // Summary:
        //     Gets the CoordinateMapper object for the sensor.
        public CoordinateMapper CoordinateMapper { get; }
        //
        // Summary:
        //     Gets or sets a filter to be applied to each depth frame.
        public IDepthFilter DepthFilter { get; set; }
        //
        // Summary:
        //     Gets the depth stream for the sensor.
        public DepthImageStream DepthStream { get; }
        //
        // Summary:
        //     Gets the USB HUB device instance id.
        public string DeviceConnectionId { get; }
        //
        // Summary:
        //     Gets or sets the desired camera elevation angle. See the documentation about
        //     the limits of this property.
        public int ElevationAngle { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether the infrared emitter is disabled.
        //      Default value of false.
        public bool ForceInfraredEmitterOff { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the Kinect is currently streaming data. To
        //     set it, call Start() or Stop().
        public bool IsRunning { get; }
        //
        // Summary:
        //     Gets the collection of all Kinect™ sensors.
        public static KinectSensorCollection KinectSensors { get; }
        //
        // Summary:
        //     Gets the maximum camera elevation angle in degrees.
        public int MaxElevationAngle { get; }
        //
        // Summary:
        //     Gets the minimum camera elevation angle in degrees.
        public int MinElevationAngle { get; }
        //
        // Summary:
        //     Gets the skeleton stream for the sensor.
        public SkeletonStream SkeletonStream { get; }
        //
        // Summary:
        //     Gets the reported status of the sensor.
        public KinectStatus Status { get; }
        //
        // Summary:
        //     Gets the unique device connection name for the sensor. This is the camera
        //     device instance id.
        public string UniqueKinectId { get; }

        // Summary:
        //     Event that fires when new frames are available from each of this Kinect sensor's
        //     active streams.
        public event EventHandler<AllFramesReadyEventArgs> AllFramesReady;
        //
        // Summary:
        //     Event that fires when a new color frame is available from this Kinect sensor.
        public event EventHandler<ColorImageFrameReadyEventArgs> ColorFrameReady;
        //
        // Summary:
        //     Event that fires when a new depth frame is available from this Kinect sensor.
        public event EventHandler<DepthImageFrameReadyEventArgs> DepthFrameReady;
        //
        // Summary:
        //     Event that fires when a new skeleton frame is available from this Kinect
        //     sensor.
        public event EventHandler<SkeletonFrameReadyEventArgs> SkeletonFrameReady;

        // Summary:
        //     Gets the current accelerometer reading of the Kinect sensor.
        //
        // Returns:
        //     A Vector4 pointing in the directon of gravity.
        //
        // Remarks:
        //     The accelerometer reading is returned as a 3d vector pointing in the direction
        //     of gravity (i.e. floor (gravity) on a non-accelerating sensor). The unit
        //     of the vector is in gravity units (g), 9.81m/s^2. The coordinate system is
        //     centered on the sensor, right-handed coordinate system with positive-Z in
        //     the direction the sensor is pointing at. In the default sensor rotation (horizontal,
        //     level placement), this will return the vector (0, -1.0, 0, 0). The w value
        //     of the Vector4 is always set to 0.0.
        public Vector4 AccelerometerGetCurrentReading();
        //
        // Summary:
        //     Frees all memory associated with the sensor. Terminates all streaming.
        public void Dispose();
        //
        // Summary:
        //     Tests whether the ColorImagePoint has a known value.
        //
        // Parameters:
        //   colorImagePoint:
        //     The ColorImagePoint to test.
        //
        // Returns:
        //     Returns true if the ColorImagePoint has a known value, false otherwise.
        public static bool IsKnownPoint(ColorImagePoint colorImagePoint);
        //
        // Summary:
        //     Tests whether the DepthImagePixel has a known value.
        //
        // Parameters:
        //   depthImagePixel:
        //     The DepthImagePixel to test.
        //
        // Returns:
        //     Returns true if the DepthImagePixel has a known value, false otherwise.
        public static bool IsKnownPoint(DepthImagePixel depthImagePixel);
        //
        // Summary:
        //     Tests whether the DepthImagePoint has a known value.
        //
        // Parameters:
        //   depthImagePoint:
        //     The DepthImagePoint to test.
        //
        // Returns:
        //     Rreturns false if depth value == 0.
        public static bool IsKnownPoint(DepthImagePoint depthImagePoint);
        //
        // Summary:
        //     Tests whether the SkeletonPoint has a known value.
        //
        // Parameters:
        //   skeletonPoint:
        //     The SkeletonPoint to test.
        //
        // Returns:
        //     Returns true if the Skeleton point has a known value, false otherwise.
        public static bool IsKnownPoint(SkeletonPoint skeletonPoint);
        //
        // Summary:
        //     Maps every point in a depth frame to the corresponding location in a ColorImageFormat
        //     coordinate space.
        //
        // Parameters:
        //   depthImageFormat:
        //     The depth format of the source.
        //
        //   depthPixelData:
        //     The depth frame pixel data, as retrieved from DepthImageFrame.CopyPixelDataTo.
        //      Must be equal in length to Width*Height of the depth format specified by
        //     depthImageFormat.
        //
        //   colorImageFormat:
        //     The desired target image format.
        //
        //   colorCoordinates:
        //     The ColorImagePoint array to receive the data. Each element will be be the
        //     result of mapping the corresponding depthPixelDatum to the specified ColorImageFormat
        //     coordinate space.  Must be equal in length to depthPixelData.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapDepthFrameToColorFrame", false)]
        public void MapDepthFrameToColorFrame(DepthImageFormat depthImageFormat, short[] depthPixelData, ColorImageFormat colorImageFormat, ColorImagePoint[] colorCoordinates);
        //
        // Summary:
        //     Maps from a depth coordinate with depth value to color coordinates.
        //
        // Parameters:
        //   depthImageFormat:
        //     The depth format of the source.
        //
        //   depthX:
        //     The X coordinate of the depth frame.
        //
        //   depthY:
        //     The Y coordinate of the depth frame.
        //
        //   depthPixelValue:
        //     The value from the depth frame's pixel data at the given coordinates (depthX,
        //     depthY).
        //
        //   colorImageFormat:
        //     The desired target image format.
        //
        // Returns:
        //     The ColorImagePoint corresponding to the point in DepthImage space.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapDepthPointToColorPoint", false)]
        public ColorImagePoint MapDepthToColorImagePoint(DepthImageFormat depthImageFormat, int depthX, int depthY, short depthPixelValue, ColorImageFormat colorImageFormat);
        //
        // Summary:
        //     Looks up the skeleton point location of the given depth X, Y.
        //
        // Parameters:
        //   depthImageFormat:
        //     The format to use in the conversion.
        //
        //   depthX:
        //     The X coordinate of the depth frame.
        //
        //   depthY:
        //     The Y coordinate of the depth frame.
        //
        //   depthPixelValue:
        //     The value from the depth frame's pixel data at the given coordinates (depthX,
        //     depthY).
        //
        // Returns:
        //     The skeleton point for the given X, Y, depth and format.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapDepthPointToSkeletonPoint", false)]
        public SkeletonPoint MapDepthToSkeletonPoint(DepthImageFormat depthImageFormat, int depthX, int depthY, short depthPixelValue);
        //
        // Summary:
        //     Looks up the color frame coordinates for a given skeleton point.
        //
        // Parameters:
        //   skeletonPoint:
        //     The supplied skeleton point.
        //
        //   colorImageFormat:
        //     The color format to convert to.
        //
        // Returns:
        //     The ColorImagePoint corresponding to the given skeleton point.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapSkeletonPointToColorPoint", false)]
        public ColorImagePoint MapSkeletonPointToColor(SkeletonPoint skeletonPoint, ColorImageFormat colorImageFormat);
        //
        // Summary:
        //     Looks up the depth frame coordinates for a given skeleton point.
        //
        // Parameters:
        //   skeletonPoint:
        //     The supplied skeleton point.
        //
        //   depthImageFormat:
        //     The depth format to convert to.
        //
        // Returns:
        //     The DepthImagePoint that contains the X, Y and depth value of the given skeleton
        //     point.
        [Obsolete("This method is replaced by Microsoft.Kinect.CoordinateMapper.MapSkeletonPointToDepthPoint", false)]
        public DepthImagePoint MapSkeletonPointToDepth(SkeletonPoint skeletonPoint, DepthImageFormat depthImageFormat);
        //
        // Summary:
        //     Start streaming data from this Kinect. This needs to be called whether polling
        //     or registering for the events. It also needs to be called to get control
        //     of the Camera.  This API may throw System.IO.IOException if the KinectSensor
        //     is already in use by another process.
        public void Start();
        //
        // Summary:
        //     Stop streaming data from this Kinect.
        public void Stop();
    }
}
