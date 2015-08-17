using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKinect.Data;

namespace TKinect.FrameManagement
{
    public class FrameRecorder
    {
        private bool _recording;
        private Stream _stream;
        private BinaryWriter _writer;

        public FrameRecorder()
        {
            _recording = false;
        }

        public void RecordHandler(object sender, TFrame frame)
        {
            if (_recording)
            {
                _writer.Write((int)frame.GetFameType());
                frame.Write(_writer);
                _writer.Flush();
            }
        }

        public void Start(Stream stream)
        {
            _stream = stream;
            _writer = new BinaryWriter(stream);
            _recording = true;
        }

        public Stream Stop()
        {
            _recording = false;
            return _stream;
        }
    }
}
