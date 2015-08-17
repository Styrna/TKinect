using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;

namespace TKinect.Display
{
    public static class SkeletonDisplayHelper
    {
        public static DrawingImage ImageSource;
        public static DrawingGroup DrawingGroup;
        public static SynchronizationContext Context { get; set; }

        private static readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        private static readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private static readonly Brush inferredJointBrush = Brushes.Yellow;
        private static readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private static readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        public static void Init()
        {
            DrawingGroup = new DrawingGroup();
            ImageSource = new DrawingImage(DrawingGroup);
            Context = SynchronizationContext.Current;
        }


        public static void SensorSkeletonFrameReady(object sender, TSkeletonFrame skeletonFrame)
        {
            Context.Send((gui) => DrawSkeletonFrame(skeletonFrame), null);
        }

        public static void DrawSkeletonFrame(TSkeletonFrame skeletonFrame)
        {
            using (DrawingContext dc = DrawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletonFrame.Skeletons.Length != 0)
                {
                    foreach (var skel in skeletonFrame.Skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == TSkeletonTrackingState.Tracked)
                        {
                            DrawBonesAndJoints(skel, dc);
                        }
                        else if (skel.TrackingState == TSkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            centerPointBrush,
                            null,
                            SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                DrawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        public static void DrawBonesAndJoints(TSkeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            DrawBone(skeleton, drawingContext, TJointType.Head, TJointType.ShoulderCenter);
            DrawBone(skeleton, drawingContext, TJointType.ShoulderCenter, TJointType.ShoulderLeft);
            DrawBone(skeleton, drawingContext, TJointType.ShoulderCenter, TJointType.ShoulderRight);
            DrawBone(skeleton, drawingContext, TJointType.ShoulderCenter, TJointType.Spine);
            DrawBone(skeleton, drawingContext, TJointType.Spine, TJointType.HipCenter);
            DrawBone(skeleton, drawingContext, TJointType.HipCenter, TJointType.HipLeft);
            DrawBone(skeleton, drawingContext, TJointType.HipCenter, TJointType.HipRight);

            // Left Arm
            DrawBone(skeleton, drawingContext, TJointType.ShoulderLeft, TJointType.ElbowLeft);
            DrawBone(skeleton, drawingContext, TJointType.ElbowLeft, TJointType.WristLeft);
            DrawBone(skeleton, drawingContext, TJointType.WristLeft, TJointType.HandLeft);

            // Right Arm
            DrawBone(skeleton, drawingContext, TJointType.ShoulderRight, TJointType.ElbowRight);
            DrawBone(skeleton, drawingContext, TJointType.ElbowRight, TJointType.WristRight);
            DrawBone(skeleton, drawingContext, TJointType.WristRight, TJointType.HandRight);

            // Left Leg
            DrawBone(skeleton, drawingContext, TJointType.HipLeft, TJointType.KneeLeft);
            DrawBone(skeleton, drawingContext, TJointType.KneeLeft, TJointType.AnkleLeft);
            DrawBone(skeleton, drawingContext, TJointType.AnkleLeft, TJointType.FootLeft);

            // Right Leg
            DrawBone(skeleton, drawingContext, TJointType.HipRight, TJointType.KneeRight);
            DrawBone(skeleton, drawingContext, TJointType.KneeRight, TJointType.AnkleRight);
            DrawBone(skeleton, drawingContext, TJointType.AnkleRight, TJointType.FootRight);

            // Render Joints
            foreach (TJoint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == TJointTrackingState.Tracked)
                {
                    drawBrush = trackedJointBrush;
                }
                else if (joint.TrackingState == TJointTrackingState.Inferred)
                {
                    drawBrush = inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        public static Point SkeletonPointToScreen(TSkeletonPoint skelpoint)
        {
            return new Point(((skelpoint.X + 1) * 320), (480 - (skelpoint.Y + 1) * 240));
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        public static void DrawBone(TSkeleton skeleton, DrawingContext drawingContext, TJointType jointType0, TJointType jointType1)
        {
            TJoint joint0 = skeleton.Joints[(int)jointType0];
            TJoint joint1 = skeleton.Joints[(int)jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TJointTrackingState.NotTracked ||
                joint1.TrackingState == TJointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == TJointTrackingState.Inferred &&
                joint1.TrackingState == TJointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = inferredBonePen;
            if (joint0.TrackingState == TJointTrackingState.Tracked && joint1.TrackingState == TJointTrackingState.Tracked)
            {
                drawPen = trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, SkeletonPointToScreen(joint0.Position), SkeletonPointToScreen(joint1.Position));
        }


        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        public static void RenderClippedEdges(TSkeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(TFrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(TFrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(TFrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(TFrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }
    }
}
