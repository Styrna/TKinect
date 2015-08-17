// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.IO;
using Microsoft.Kinect;

namespace TKinect.Data.SkeletonData
{
    public class TSkeletonFrame : TFrame
	{
		public Tuple<float,float,float,float> FloorClipPlane { get; set; }
        public int SkeletonArrayLength { get; set; }

        public TSkeleton[] Skeletons { get; set; }

        public TSkeletonFrame()
        {
            
        }

        public TSkeletonFrame(SkeletonFrame sensorFrame)
        {
            var skeletonData = new Skeleton[sensorFrame.SkeletonArrayLength];
            sensorFrame.CopySkeletonDataTo(skeletonData);

            var skeletons = new TSkeleton[sensorFrame.SkeletonArrayLength];
            for (int i = 0; i < sensorFrame.SkeletonArrayLength; i++)
                skeletons[i] = new TSkeleton(skeletonData[i]);

            Skeletons = skeletons;

            FloorClipPlane = sensorFrame.FloorClipPlane;
            SkeletonArrayLength = sensorFrame.SkeletonArrayLength;
            FrameNumber = sensorFrame.FrameNumber;
            Timestamp = sensorFrame.Timestamp;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Timestamp);
            writer.Write(FrameNumber);

            writer.Write(FloorClipPlane.Item1);
            writer.Write(FloorClipPlane.Item2);
            writer.Write(FloorClipPlane.Item3);
            writer.Write(FloorClipPlane.Item4);

            writer.Write(SkeletonArrayLength);

            foreach (var skeleton in Skeletons)
                skeleton.Write(writer);
        }

        public override void Read(BinaryReader reader)
        {
            Timestamp = reader.ReadInt64();
            FrameNumber = reader.ReadInt32();

            FloorClipPlane = new Tuple<float, float, float, float>(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            SkeletonArrayLength = reader.ReadInt32();

            Skeletons = new TSkeleton[SkeletonArrayLength];
            for (int i = 0; i < Skeletons.Length; i++)
            {
                Skeletons[i] = new TSkeleton();
                Skeletons[i].Read(reader);
            }
        }

        public override TFrameType GetFameType()
        {
            return TFrameType.TSkeletonFrame;
        }
    }
}
