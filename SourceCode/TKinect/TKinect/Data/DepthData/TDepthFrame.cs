// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.IO;
using Microsoft.Kinect;

namespace TKinect.Data.DepthData
{
    public class TDepthFrame : TFrame
	{
        public int PixelDataLength { get; set; }
        public int BytesPerPixel { get; set; }
        public short[] DepthData { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int MinDepth { get; set; }
        public int MaxDepth { get; set; }

        public TDepthFrame()
        {
            
        }

        public TDepthFrame(DepthImageFrame sensorFrame)
        {
            //TODO This can be done better
            var depthImagePixels = new DepthImagePixel[sensorFrame.PixelDataLength];
            sensorFrame.CopyDepthImagePixelDataTo(depthImagePixels);

            var depthData = new short[sensorFrame.PixelDataLength];

            for (int i = 0; i < sensorFrame.PixelDataLength; i++)
                depthData[i] = depthImagePixels[i].Depth;

            DepthData = depthData;

            PixelDataLength = sensorFrame.PixelDataLength;
            BytesPerPixel = sensorFrame.BytesPerPixel;
            FrameNumber = sensorFrame.FrameNumber;
            Width = sensorFrame.Width;
            Height = sensorFrame.Height;
            Timestamp = sensorFrame.Timestamp;

            MinDepth = sensorFrame.MinDepth;
            MaxDepth = sensorFrame.MaxDepth;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Timestamp);
            writer.Write(FrameNumber);

            writer.Write(PixelDataLength);
            writer.Write(BytesPerPixel);

            for(int i=0;i<PixelDataLength; i++)
                writer.Write(DepthData[i]);

            writer.Write(Width);
            writer.Write(Height);

            writer.Write(MinDepth);
            writer.Write(MaxDepth);
        }

        public override void Read(BinaryReader reader)
        {
            Timestamp = reader.ReadInt64();
            FrameNumber = reader.ReadInt32();

            PixelDataLength = reader.ReadInt32();
            BytesPerPixel = reader.ReadInt32();

            DepthData = new short[PixelDataLength];
            for (int i = 0; i < PixelDataLength; i++)
                DepthData[i] = reader.ReadInt16();
            
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();

            MinDepth = reader.ReadInt32();
            MaxDepth = reader.ReadInt32();
        }

        public override TFrameType GetFameType()
        {
            return TFrameType.TDepthFrame;
        }
    }
}