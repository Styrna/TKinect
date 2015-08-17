using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TKinect.Data;
using TKinect.Data.ColorData;

namespace TKinectTests.Data
{
    public abstract class TFrameTests<T> where T : TFrame
    {
        protected T startFrame;
        protected Random Random;

        protected byte[] Write(TFrame frame)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryWriter = new BinaryWriter(memoryStream);
                frame.Write(binaryWriter);
                return memoryStream.ToArray();
            }
        }

        protected T Read(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                var binaryReader = new BinaryReader(memoryStream);
                var frame = Activator.CreateInstance<T>();
                frame.Read(binaryReader);
                return frame;
            }
        }

        protected abstract T InitializeFrame();
        protected abstract void CompareFrames(T frame1, T frame2);

        [SetUp]
        public void Init()
        {
            Random = new Random();
            startFrame = InitializeFrame();
        }

        [Test]
        public void WriteReadFrameTest()
        {
            var data = Write(startFrame);
            var endFrame = Read(data);

            CompareFrames(startFrame,endFrame);
        }
    }
}
