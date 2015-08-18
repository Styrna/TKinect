using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using TKinect;

namespace Model
{
    public static class PlatformInitializer
    {
        public static void Initialize()
        {
            DependencyFactory.Container.RegisterInstance<IViewConfiguration>(new ViewConfiguration());
            DependencyFactory.Container.RegisterInstance<IMapDefinition>(new MapDefinition());

            InitializeKinect();
        }

        private static void InitializeKinect()
        {
            int index = 0;

            KinectSensor kinectSensor = null;
            var testKinect = new TKinect.TKinect();
            DependencyFactory.Container.RegisterInstance(testKinect);

            while (kinectSensor == null && index < KinectSensor.KinectSensors.Count)
            {
                try
                {
                    kinectSensor = KinectSensor.KinectSensors[index];

                    kinectSensor.Start();
                }
                catch (Exception)
                {
                    kinectSensor = null;
                }

                index++;
            }
            if (kinectSensor != null)
            {
                kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters
                {
                    Smoothing = 0.5f,
                    Correction = 0.5f,
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                });
                kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);


                kinectSensor.SkeletonFrameReady += testKinect.SensorSkeletonFrameHandler;
                kinectSensor.ColorFrameReady += testKinect.SensorColorFrameHandler;
                kinectSensor.DepthFrameReady += testKinect.SensorDepthFrameHandler;
                
                DependencyFactory.Container.RegisterInstance(kinectSensor);
                DependencyFactory.Container.RegisterInstance(testKinect);
            }
        }

        private static void UninitializeKinect()
        {
            foreach (var kinectSensor in KinectSensor.KinectSensors)
            {
                kinectSensor.Stop();
                kinectSensor.Dispose();
            }

            var kinectSensor2 = DependencyFactory.Container.Resolve<KinectSensor>();
            DependencyFactory.Container.Teardown(kinectSensor2);

        }

        public static void Teardown()
        {
            UninitializeKinect();
        }
    }
}
