using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace TKinect.Data.SkeletonData
{
    public class TSkeletonPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public TSkeletonPoint() {}
        public TSkeletonPoint(SkeletonPoint skeletonPoint)
        {
            X = skeletonPoint.X;
            Y = skeletonPoint.Y;
            Z = skeletonPoint.Z;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public void Read(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }
    }
}
