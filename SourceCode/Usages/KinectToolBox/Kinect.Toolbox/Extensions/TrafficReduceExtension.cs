using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect.Toolbox.Extensions
{
    public class TrafficReduceExtension : IAnalyzeExtension
    {
        private int CountToSkip, CurrentCount;
        public TrafficReduceExtension(int countToSkip)
        {
            this.CountToSkip = countToSkip;
            this.CurrentCount = countToSkip;
        }

        private bool ShouldSkip()
        {
            if (CurrentCount == 0)
            {
                CurrentCount = this.CountToSkip;
                return false;
            }
            CurrentCount--;
            return true;
        }

        public bool CanAnalyze()
        {
            return !ShouldSkip();
        }

        public bool CanRaise()
        {
            return true;
        }
    }
}
