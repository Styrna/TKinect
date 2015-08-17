using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using TKinect.Data.ColorData;

namespace TKinect.Display
{
    public static class ColorDisplayHelper
    {
        public static WriteableBitmap ColorBitmap { get; set; }
        public static SynchronizationContext Context { get; set; }

        //Init Color Data
        public static void Init()
        {
            ColorBitmap = new WriteableBitmap(640, 480, 96.0, 96.0, PixelFormats.Bgr32, null);
            Context = SynchronizationContext.Current;
        }

        public static void SensorColorFrameReady(object sender, TColorFrame colorFrame)
        {
            Context.Send((gui) => DrawColorFrame(colorFrame), null);
        }

        //Draw Color Frame
        public static void DrawColorFrame(TColorFrame colorData)
        {
            ColorBitmap.WritePixels(
                new Int32Rect(0, 0, ColorBitmap.PixelWidth, ColorBitmap.PixelHeight),
                colorData.ColorData,
                ColorBitmap.PixelWidth*sizeof (int),
                0);
        }
    }
}
