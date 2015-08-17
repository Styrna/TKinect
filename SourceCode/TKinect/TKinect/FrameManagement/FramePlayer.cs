using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TKinect.Data;

namespace TKinect.FrameManagement
{
    public class FramePlayer<T> where T : TFrame
    {
        public event EventHandler<T> FrameReady;
        public List<T> Frames;

        private CancellationTokenSource _cancellationTokenSource;

        public FramePlayer()
        {
            Frames = new List<T>();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var cancelToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(() =>
            {
                if (!Frames.Any())
                    return;
                var lastFrameTime = Frames.First().Timestamp;
                foreach (T frame in Frames)
                {
                    //Speed controll
                    Thread.Sleep(TimeSpan.FromMilliseconds(frame.Timestamp - lastFrameTime));
                    lastFrameTime = frame.Timestamp;

                    if (cancelToken.IsCancellationRequested)
                        break;

                    FrameReady(this, frame);
                }
            }, cancelToken);
        }

        public void Stop()
        {
            if (_cancellationTokenSource == null)
                return;
            _cancellationTokenSource.Cancel();
        }

        public void Clear()
        {
            Stop();
            Frames.Clear();
        }
    }
}
