using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IViewConfiguration
    {
        bool IsHandNavigationMode { get; set; }
        bool IsHeadNavigationMode { get; set; }
    }

    public class ViewConfiguration : IViewConfiguration
    {
        public bool IsHandNavigationMode { get; set; }
        public bool IsHeadNavigationMode { get; set; }
    }
}
