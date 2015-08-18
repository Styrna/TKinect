using System;
using System.Collections.Generic;
using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Console;

namespace Kinect.Toolbox.Analyzers
{
    public abstract class FramesAnalyzer
    {
        private readonly List<IAnalyzeExtension> _analyzeExtensions;

        protected FramesCollector _framesCollector;

        public long LastFrameTimestamp { get; set; }

        protected FramesAnalyzer(FramesCollector framesCollector)
        {
            _analyzeExtensions = new List<IAnalyzeExtension>();
            _framesCollector = framesCollector;
            _framesCollector.FrameReady += FramesCollectorOnFrameReady;
        }

        public void AddBeforeAnalyzeExtension(IAnalyzeExtension analyzeExtension)
        {
            _analyzeExtensions.Add(analyzeExtension);
        }

        protected abstract void Analyze(FrameReadyEventArgs frameReadyEventArgs);

        private void FramesCollectorOnFrameReady(object sender, FrameReadyEventArgs frameReadyEventArgs)
        {
            if (_analyzeExtensions.Any(a => !a.CanAnalyze()))
            {
                return;
            }

            var milis = DateTime.Now.Ticks;

            Analyze(frameReadyEventArgs);
            
            var passed = DateTime.Now.Ticks - milis;
            
            if (passed > -1)
            {
                //LogString.Log(this.GetType().Name + " took: " + passed);
            }
            
        }

        protected void Raise(Action action)
        {
            if (_analyzeExtensions.Any(a => !a.CanRaise()))
            {
                return;
            }

            action();

            LastFrameTimestamp = DateTime.Now.Ticks;
        }
    }
}
