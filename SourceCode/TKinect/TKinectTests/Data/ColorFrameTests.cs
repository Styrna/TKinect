using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TKinect.Data;
using TKinect.Data.ColorData;

namespace TKinectTests.Data
{
    [TestFixture]
    public class ColorFrameTests : TFrameTests<TColorFrame>
    {
        protected override TColorFrame InitializeFrame()
        {
            var array = new byte[10];
            Random random = new Random();
            random.NextBytes(array);

            return new TColorFrame()
            {
                Width = 640,
                Height = 480,
                Timestamp = 60,
                FrameNumber = 1,
                PixelDataLength = 10,
                BytesPerPixel = 4,
                ColorData = array
            };
        }

        protected override void CompareFrames(TColorFrame frame1, TColorFrame frame2)
        {
            Assert.True(frame1.Width == frame2.Width);
            Assert.True(frame1.Height == frame2.Height);
            Assert.True(frame1.Timestamp == frame2.Timestamp);
            Assert.True(frame1.FrameNumber == frame2.FrameNumber);
            Assert.True(frame1.PixelDataLength == frame2.PixelDataLength);
            Assert.True(frame1.BytesPerPixel == frame2.BytesPerPixel);

            Assert.True(frame1.ColorData.SequenceEqual(frame2.ColorData));
        }
    }
}
