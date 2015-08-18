using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect.Toolbox.Extensions
{
    public interface IAnalyzeExtension
    {
        bool CanAnalyze();
        bool CanRaise();
    }
}
