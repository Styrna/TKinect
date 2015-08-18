using System;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace Kinect.Toolbox
{
    public class Entry
    {
        public Joint Joint { get; set; }
        public DateTime Time { get; set; }
        public Vector3 Position { get; set; }
        public Ellipse DisplayEllipse { get; set; }
    }
}
