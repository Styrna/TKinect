using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;
using NUnit.Framework;
using TKinect;

namespace Tests
{
    [TestFixture]
    public class HelloWorldAnalyzerTests
    {
        private TKinect.TKinect Kinect { get; set;}
        private PostureAnalyzer PostureAnalyzer { get; set; }

        [SetUp]
        public void Setup()
        {
            Configuration.postureNavigation = true;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            
            Kinect = new TKinect.TKinect();
            var framesCollector = new FramesCollector(Kinect, 30);

            PostureAnalyzer = new PostureAnalyzer(framesCollector, 25);
            PostureAnalyzer.PostureDetected += PostureAnalyzerOnPostureDetected;
        }

        private bool _leftHello = false;
        private void PostureAnalyzerOnPostureDetected(object sender, PostureEventArgs args)
        {
            if (args.Posture == PosturesEnum.LeftHello)
                _leftHello = true;
        }

        [Test]
        public void LeftHelloDetected()
        {
            using (var fileStream = new FileStream(@"Recordings\RECORDING.xed", FileMode.Open))
            {
                Kinect.RecordStart(fileStream);
            }

            Thread.Sleep(3000);

            Assert.True(_leftHello);
        }
    }
}
