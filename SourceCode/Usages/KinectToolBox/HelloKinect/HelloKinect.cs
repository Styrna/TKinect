using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TKinect;

namespace HelloKinect
{
    public class HelloKinect
    {
        private bool _end = false;
        
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

        public bool RunKinect()
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
                return true;
            }
            Console.WriteLine("NO REAL KINECT DETECTED");
            return false;
        }

        private void PostureAnalyzerOnPostureDetected(object sender, PostureEventArgs args)
        {
            if(args.Posture == PosturesEnum.RightHello)
            {
                HelloCounter++;
                Console.WriteLine(string.Format("Hello nr {0} Detected !!!", HelloCounter));
            }
        }

    }
}
