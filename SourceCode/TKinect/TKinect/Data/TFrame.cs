using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKinect.Data
{
    public abstract class TFrame
    {
        public int FrameNumber { get; set; }
        public long Timestamp { get; set; }

        public abstract void Read(BinaryReader reader);
        public abstract void Write(BinaryWriter writer);

        public abstract TFrameType GetFameType();
    }
}
