using System.Linq;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Utils;

namespace Kinect.Toolbox.Extensions
{
    public class SkeletonStabilityAnalyzeExtension : IAnalyzeExtension
    {
        private readonly FramesCollector _framesCollector;

        protected float Threshold { get; set; }
        
        public SkeletonStabilityAnalyzeExtension(FramesCollector framesCollector, float threshold = 0.05f)
        {
            _framesCollector = framesCollector;
            Threshold = threshold;
        }

        public bool CanAnalyze()
        {
            return IsStable();
        }

        public bool CanRaise()
        {
            return true;
        }

        private bool IsStable()
        {
            var frames = _framesCollector.GetFrames().Select(a => a.GetNearestSkeleton()).ToList();

            Vector3 current = frames[frames.Count - 1].Position.ToVector3();

            Vector3 average = Vector3.Zero;

            for (int index = 0; index < frames.Count - 2; index++)
            {
                average += frames[index].Position.ToVector3();
            }

            average /= frames.Count - 1;

            return !((average - current).Length > Threshold);
        }
    }
}
