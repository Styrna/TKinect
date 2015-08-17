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
using TKinect.Data.DepthData;

namespace TKinect.Display
{
    public static class DepthDisplayHelper
    {
        public static byte[] ColorPixels;
        public static WriteableBitmap DepthBitmap { get; set; }
        public static SynchronizationContext Context { get; set; }

        public static void Init()
        {
            ColorPixels = new byte[640 * 480 * sizeof(int)];
            DepthBitmap = new WriteableBitmap(640, 480, 96.0, 96.0, PixelFormats.Bgr32, null);
            Context = SynchronizationContext.Current;
        }

        public static void SensorDepthFrameReady(object sender, TDepthFrame depthFrame)
        {
            Context.Send((gui) => DrawDepthFrame(depthFrame), null);
        }

        //Draw Depth Frame
        public static void DrawDepthFrame(TDepthFrame depthFrame)
        {
            int minDepth = depthFrame.MinDepth;
            int maxDepth = depthFrame.MaxDepth;

            // Convert the depth to RGB
            int colorPixelIndex = 0;
            for (int i = 0; i < depthFrame.DepthData.Length; ++i)
            {
                short depth = depthFrame.DepthData[i];
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                ColorPixels[colorPixelIndex++] = intensity;
                ColorPixels[colorPixelIndex++] = intensity;
                ColorPixels[colorPixelIndex++] = intensity;

                ++colorPixelIndex;
            }

            DepthBitmap.WritePixels(
                new Int32Rect(0, 0, DepthBitmap.PixelWidth, DepthBitmap.PixelHeight),
                ColorPixels,
                DepthBitmap.PixelWidth * sizeof(int),
                0);
        }
    }
}
