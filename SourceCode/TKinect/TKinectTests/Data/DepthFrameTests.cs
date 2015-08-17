using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TKinect.Data.DepthData;

namespace TKinectTests.Data
{
    [TestFixture]
    public class DepthFrameTests : TFrameTests<TDepthFrame>
    {
        protected override TDepthFrame InitializeFrame()
        {
            return new TDepthFrame()
            {
                Width = 640,
                Height = 480,
                Timestamp = 60,
                FrameNumber = 1,
                PixelDataLength = 10,
                BytesPerPixel = 4,
                DepthData = new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
            };
        }

        protected override void CompareFrames(TDepthFrame frame1, TDepthFrame frame2)
        {
            Assert.True(frame1.Width == frame2.Width);
            Assert.True(frame1.Height == frame2.Height);
            Assert.True(frame1.Timestamp == frame2.Timestamp);
            Assert.True(frame1.FrameNumber == frame2.FrameNumber);
            Assert.True(frame1.PixelDataLength == frame2.PixelDataLength);
            Assert.True(frame1.BytesPerPixel == frame2.BytesPerPixel);

            Assert.True(frame1.DepthData.SequenceEqual(frame2.DepthData));
        }
    }
}
