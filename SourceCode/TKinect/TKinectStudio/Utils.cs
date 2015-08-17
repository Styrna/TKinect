using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using TKinect;
using TKinect.Display;
using TKinectStudio.KinectViews;

namespace TKinectStudio
{
    public static class Utils
    {
        public static KinectSensor Sensor { get; set; }
        public static TKinect.TKinect TKinect { get; set; }

        public static void InitKinectSensor(KinectSensor sensor)
        {
            //COLOR 
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.ColorFrameReady += TKinect.SensorColorFrameHandler;

            //DEPTH
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.DepthFrameReady += TKinect.SensorDepthFrameHandler;

            //SKELETON
            sensor.SkeletonStream.Enable();
            sensor.SkeletonFrameReady += TKinect.SensorSkeletonFrameHandler;

            //Redirect real sensor frames to TKinect
            sensor.ColorFrameReady += TKinect.SensorColorFrameHandler;
            sensor.DepthFrameReady += TKinect.SensorDepthFrameHandler;
            sensor.SkeletonFrameReady += TKinect.SensorSkeletonFrameHandler;

            try
            {
                Sensor.Start();
            }
            catch (IOException)
            {
                Sensor = null;
            }
        }

        public static void InitKinectHost()
        {
            //Initialize TKinect
            TKinect = new TKinect.TKinect();

            //COLOR
            ColorDisplayHelper.Init();
            TKinect.ColorFrameReady += ColorDisplayHelper.SensorColorFrameReady;

            //DEPTH
            DepthDisplayHelper.Init();
            TKinect.DepthFrameReady += DepthDisplayHelper.SensorDepthFrameReady;

            //SKELETON
            SkeletonDisplayHelper.Init();
            TKinect.SkeletonFrameReady += SkeletonDisplayHelper.SensorSkeletonFrameReady;

            //HOST
            TKinect.ColorFrameHost.Start(4501);
            TKinect.DepthFrameHost.Start(4502);
            TKinect.SkeletonFrameHost.Start(4503);
        }

        public static void InitKinectClient()
        {
            TKinect = new TKinect.TKinect();

            //COLOR 
            ColorDisplayHelper.Init();
            TKinect.ColorFrameReady += ColorDisplayHelper.SensorColorFrameReady;

            //DEPTH
            DepthDisplayHelper.Init();
            TKinect.DepthFrameReady += DepthDisplayHelper.SensorDepthFrameReady;

            //SKELETON
            SkeletonDisplayHelper.Init();
            TKinect.SkeletonFrameReady += SkeletonDisplayHelper.SensorSkeletonFrameReady;

            //CLINET
            TKinect.ColorFrameClient.Connect("localhost",4501);
            TKinect.DepthFrameClient.Connect("localhost", 4502);
            TKinect.SkeletonFrameClient.Connect("localhost", 4503);
        }
    }
}
