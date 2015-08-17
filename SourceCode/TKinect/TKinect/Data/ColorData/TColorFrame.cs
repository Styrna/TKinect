// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.IO;
using Microsoft.Kinect;

namespace TKinect.Data.ColorData
{
    public class TColorFrame : TFrame
	{
        public int PixelDataLength { get; set; }
        public int BytesPerPixel { get; set; }
        public byte[] ColorData { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public TColorFrame()
        {
            
        }

        public TColorFrame(ColorImageFrame sensorFrame)
        {
            ColorData = sensorFrame.GetRawPixelData();

            PixelDataLength = sensorFrame.PixelDataLength;
            BytesPerPixel = sensorFrame.BytesPerPixel;
            FrameNumber = sensorFrame.FrameNumber;
            Width = sensorFrame.Width;
            Height = sensorFrame.Height;
            Timestamp = sensorFrame.Timestamp;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(Timestamp);
            writer.Write(FrameNumber);

            writer.Write(PixelDataLength);
            writer.Write(BytesPerPixel);
            writer.Write(ColorData);
            
            writer.Write(Width);
            writer.Write(Height);
        }

        public override void Read(BinaryReader reader)
        {
            Timestamp = reader.ReadInt64();
            FrameNumber = reader.ReadInt32();

            PixelDataLength = reader.ReadInt32();
            BytesPerPixel = reader.ReadInt32();
            ColorData = reader.ReadBytes(PixelDataLength);

            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
        }

        public override TFrameType GetFameType()
        {
            return TFrameType.TColorFrame;
        }
    }
}