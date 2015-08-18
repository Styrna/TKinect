using Microsoft.Kinect;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Utils
{
    public static class Tools
    {
        public static Vector2 Convert( TSkeletonPoint position)
        {
            float width = 0;
            float height = 0;
            float x = 0;
            float y = 0;

            //if (sensor.ColorStream.IsEnabled)
            //{
            //    var colorPoint = sensor.MapSkeletonPointToColor(position, sensor.ColorStream.Format);
            //    x = colorPoint.X;
            //    y = colorPoint.Y;

            //    switch (sensor.ColorStream.Format)
            //    {
            //        case ColorImageFormat.RawYuvResolution640x480Fps15:
            //        case ColorImageFormat.RgbResolution640x480Fps30:
            //        case ColorImageFormat.YuvResolution640x480Fps15:
            //            width = 640;
            //            height = 480;
            //            break;
            //        case ColorImageFormat.RgbResolution1280x960Fps12:
            //            width = 1280;
            //            height = 960;
            //            break;
            //    }
            //}
            //else if (sensor.DepthStream.IsEnabled)
            //{
            //    var depthPoint = sensor.MapSkeletonPointToDepth(position, sensor.DepthStream.Format);
            //    x = depthPoint.X;
            //    y = depthPoint.Y;

            //    switch (sensor.DepthStream.Format)
            //    {
            //        case DepthImageFormat.Resolution80x60Fps30:
            //            width = 80;
            //            height = 60;
            //            break;
            //        case DepthImageFormat.Resolution320x240Fps30:
            //            width = 320;
            //            height = 240;
            //            break;
            //        case DepthImageFormat.Resolution640x480Fps30:
            //            width = 640;
            //            height = 480;
            //            break;
            //    }
            //}
            //else
            //{
            width = 640;
            height = 480;
            //}

            return new Vector2(x / width, y / height);
        }
    }
}
