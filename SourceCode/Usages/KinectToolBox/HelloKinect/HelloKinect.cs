using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TKinect;

namespace HelloKinect
{
    public class HelloKinect
    {
        public delegate void HelloHandler(object sender, int helloCount);
        public event HelloHandler HelloDetected;

        private const string RecordingPath = @"C:\Users\Styrna\Desktop\PassedData\hello.xed";

        public int HelloCounter { get; private set; }

        public KinectSensor Kinect { get; set; }
        public TKinect.TKinect TKinect { get; set; }
        public PostureAnalyzer PostureAnalyzer { get; set; }

        public HelloKinect()
        {
            //Initialize TKinect
            TKinect = new TKinect.TKinect();
            
            //Initialize KinectToolBox frame collector
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var framesCollector = new FramesCollector(TKinect, 30);

            //Create and use one of analyzers
            PostureAnalyzer = new PostureAnalyzer(framesCollector, 25);
            Configuration.postureNavigation = true;
            PostureAnalyzer.PostureDetected += PostureAnalyzerOnPostureDetected;
        }

        public void RunRealKinect()
        {
            int index = 0;
            while (Kinect == null && index < KinectSensor.KinectSensors.Count)
            {
                try
                {
                    Kinect = KinectSensor.KinectSensors[index];
                    Kinect.Start();
                }
                catch (Exception e)
                {
                    Kinect = null;
                }
                index++;
            }
            if (Kinect != null)
            {
                Kinect.SkeletonStream.Enable(new TransformSmoothParameters
                {
                    Smoothing = 0.5f,
                    Correction = 0.5f,
                    Prediction = 0.5f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                });
                Kinect.SkeletonFrameReady += TKinect.SensorSkeletonFrameHandler;
                return;
            }
            Console.WriteLine("NO REAL KINECT DETECTED");
        }

        public void RunKinectReplay()
        {
            var fileStream = new FileStream(RecordingPath, FileMode.Open);
            TKinect.ReplayStart(fileStream);
        }

        public void RunKinectClient()
        {
            TKinect.SkeletonFrameClient.Connect("localhost", 4503);
        }

        private void PostureAnalyzerOnPostureDetected(object sender, PostureEventArgs args)
        {
            if(args.Posture == PosturesEnum.RightHello)
            {
                HelloCounter++;
                HelloDetected(this, HelloCounter);
            }
        }

    }
}
