using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TKinect.Data;
using TKinect.Data.ColorData;
using TKinect.Data.DepthData;
using TKinect.Data.SkeletonData;

namespace TKinect.FrameManagement
{
    public class FrameReplayer
    {
        public event EventHandler<TColorFrame> ColorFrameReady;
        public event EventHandler<TDepthFrame> DepthFrameReady;
        public event EventHandler<TSkeletonFrame> SkeletonFrameReady;

        public event EventHandler<string> ReplayEnded;

        private CancellationTokenSource _cancellationTokenSource;

        public FrameReplayer()
        {
        }

        public void Play(Stream stream)
        {
            _lastFrameTime = 0;
            var reader = new BinaryReader(stream);

            _cancellationTokenSource = new CancellationTokenSource();
            var cancelToken = _cancellationTokenSource.Token;

            Task.Factory.StartNew(() =>
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    var frameType = (TFrameType)reader.ReadInt32();
                    switch (frameType)
                    {
                        case TFrameType.TColorFrame:
                            var colorFrame = new TColorFrame();
                            colorFrame.Read(reader);
                            SpeedControll(colorFrame.Timestamp);
                            ColorFrameReady(this, colorFrame);
                            break;
                        case TFrameType.TDepthFrame:
                            var depthFrame = new TDepthFrame();
                            depthFrame.Read(reader);
                            SpeedControll(depthFrame.Timestamp);
                            DepthFrameReady(this, depthFrame);
                            break;
                        case TFrameType.TSkeletonFrame:
                            var skeletonFrame = new TSkeletonFrame();
                            skeletonFrame.Read(reader);
                            SpeedControll(skeletonFrame.Timestamp);
                            SkeletonFrameReady(this, skeletonFrame);
                            break;
                    }


                }
                reader.Close();
                reader.Dispose();
                 
                if (ReplayEnded != null)
                    ReplayEnded(this,"Ended");
            }, cancelToken);
        }

        private long _lastFrameTime;
        private void SpeedControll(long frameTime)
        {
            if (_lastFrameTime == 0 || _lastFrameTime > frameTime)
                _lastFrameTime = frameTime;

            Thread.Sleep(TimeSpan.FromMilliseconds(frameTime - _lastFrameTime));
            _lastFrameTime = frameTime;
        }
    }
}
