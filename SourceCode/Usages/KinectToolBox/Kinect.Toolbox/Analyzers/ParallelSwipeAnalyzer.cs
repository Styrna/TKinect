using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect.Toolbox.Collectors;
using Microsoft.Kinect;
using Kinect.Toolbox.Utils;
using Console;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.Analyzers
{
    public class ParallelSwipeAnalyzer : FramesAnalyzer
    {
        private SwipeGestureWrapper _leftHandGestrue;
        private SwipeGestureWrapper _rightHandGestrue;
        private SwipeAnalyzer _rightHandSwipeAnalyzer;
        private SwipeAnalyzer _leftHandSwipeAnalyzer;

        public long Epsilon { get; set; }

        public delegate void ParallelSwipeDetectedHandler(object sender, SwipeEventArgs args);
        public event ParallelSwipeDetectedHandler ParallelSwipeDetected;

        public ParallelSwipeAnalyzer(FramesCollector framesCollector, long epsilon = 1000) : base(framesCollector)
        {
            Epsilon = epsilon;

            _leftHandSwipeAnalyzer = new SwipeAnalyzer(framesCollector, TJointType.HandLeft);
            _leftHandSwipeAnalyzer.SwipeDetected += (sender, args) => { _leftHandGestrue = new SwipeGestureWrapper(args.Gesture, DateTime.Now.Ticks); };

            _rightHandSwipeAnalyzer = new SwipeAnalyzer(framesCollector, TJointType.HandRight);
            _rightHandSwipeAnalyzer.SwipeDetected += (sender, args) => { _rightHandGestrue = new SwipeGestureWrapper(args.Gesture, DateTime.Now.Ticks); };
        }

        protected override void Analyze(FrameReadyEventArgs frameReadyEventArgs)
        {
            if (!Configuration.dualSwipeNavigation) return;
            try
            {
                if (Math.Abs(_leftHandGestrue.Timestamp - _rightHandGestrue.Timestamp) > Epsilon)
                {
                    return;
                }

                if (_leftHandGestrue.Gesture == GesturesEnum.SwipeToLeft && _rightHandGestrue.Gesture == GesturesEnum.SwipeToRight)
                {
                    LogString.Log("Event: SwipeOut");
                    Raise(() => ParallelSwipeDetected(this, new SwipeEventArgs(GesturesEnum.SwipeOut)));
                }
                if (_leftHandGestrue.Gesture == GesturesEnum.SwipeToRight && _rightHandGestrue.Gesture == GesturesEnum.SwipeToLeft)
                {
                    LogString.Log("Event: SwipeIn");
                    Raise(() => ParallelSwipeDetected(this, new SwipeEventArgs(GesturesEnum.SwipeIn)));
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine("wacek");
            }
        }
    }
    class SwipeGestureWrapper
    {
        public GesturesEnum Gesture { get; private set; }
        public long Timestamp { get; private set; }

        public SwipeGestureWrapper(GesturesEnum gesture, long timestamp)
        {
            Gesture = gesture;
            Timestamp = timestamp;
        }
    }
}
