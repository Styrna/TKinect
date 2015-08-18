using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Media;
using Kinect.Toolbox.Utils;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;

namespace Kinect.Toolbox.UI
{
    public class SkeletonDisplayManager
    {
        readonly Canvas rootCanvas;
        //readonly KinectSensor sensor;

        public SkeletonDisplayManager(Canvas root)
        {
            rootCanvas = root;
            //sensor = kinectSensor;
        }

        void GetCoordinates(TJointType jointType, IEnumerable<TJoint> joints, out float x, out float y)
        {
            var joint = joints.First(j => j.JointType == jointType);

            Vector2 vector2 = Tools.Convert(joint.Position);

            x = (float)(vector2.X * rootCanvas.ActualWidth);
            y = (float)(vector2.Y * rootCanvas.ActualHeight);
        }

        void Plot(TJointType centerID, IEnumerable<TJoint> joints)
        {
            float centerX;
            float centerY;

            GetCoordinates(centerID, joints, out centerX, out centerY);

            const double diameter = 8;

            Ellipse ellipse = new Ellipse
            {
                Width = diameter,
                Height = diameter,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                StrokeThickness = 4.0,
                Stroke = new SolidColorBrush(Colors.Green),
                StrokeLineJoin = PenLineJoin.Round
            };

            Canvas.SetLeft(ellipse, centerX - ellipse.Width / 2);
            Canvas.SetTop(ellipse, centerY - ellipse.Height / 2);

            rootCanvas.Children.Add(ellipse);
        }

        void Plot(TJointType centerID, TJointType baseID, TJoint[] joints)
        {
            float centerX;
            float centerY;

            GetCoordinates(centerID, joints, out centerX, out centerY);

            float baseX;
            float baseY;

            GetCoordinates(baseID, joints, out baseX, out baseY);

            double diameter = Math.Abs(baseY - centerY);

            Ellipse ellipse = new Ellipse
            {
                Width = diameter,
                Height = diameter,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                StrokeThickness = 4.0,
                Stroke = new SolidColorBrush(Colors.Green),
                StrokeLineJoin = PenLineJoin.Round
            };

            Canvas.SetLeft(ellipse, centerX - ellipse.Width / 2);
            Canvas.SetTop(ellipse, centerY - ellipse.Height / 2);

            rootCanvas.Children.Add(ellipse);
        }

        void Trace(TJointType sourceID, TJointType destinationID, TJoint[] joints)
        {
            float sourceX;
            float sourceY;

            GetCoordinates(sourceID, joints, out sourceX, out sourceY);

            float destinationX;
            float destinationY;

            GetCoordinates(destinationID, joints, out destinationX, out destinationY);

            Line line = new Line
                            {
                                X1 = sourceX,
                                Y1 = sourceY,
                                X2 = destinationX,
                                Y2 = destinationY,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Top,
                                StrokeThickness = 4.0,                                
                                Stroke = new SolidColorBrush(Colors.Green),
                                StrokeLineJoin = PenLineJoin.Round
                            };


            rootCanvas.Children.Add(line);
        }

        public void Draw(TSkeleton[] skeletons, bool seated)
        {
            if (rootCanvas == null)
            {
                return;
            }
            rootCanvas.Children.Clear();
            foreach (TSkeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState != TSkeletonTrackingState.Tracked)
                    continue;

                Plot(TJointType.HandLeft, skeleton.Joints);
                Trace(TJointType.HandLeft, TJointType.WristLeft, skeleton.Joints);
                Plot(TJointType.WristLeft, skeleton.Joints);
                Trace(TJointType.WristLeft, TJointType.ElbowLeft, skeleton.Joints);
                Plot(TJointType.ElbowLeft, skeleton.Joints);
                Trace(TJointType.ElbowLeft, TJointType.ShoulderLeft, skeleton.Joints);
                Plot(TJointType.ShoulderLeft, skeleton.Joints);
                Trace(TJointType.ShoulderLeft, TJointType.ShoulderCenter, skeleton.Joints);
                Plot(TJointType.ShoulderCenter, skeleton.Joints);

                Trace(TJointType.ShoulderCenter, TJointType.Head, skeleton.Joints);

                Plot(TJointType.Head, TJointType.ShoulderCenter, skeleton.Joints);

                Trace(TJointType.ShoulderCenter, TJointType.ShoulderRight, skeleton.Joints);
                Plot(TJointType.ShoulderRight, skeleton.Joints);
                Trace(TJointType.ShoulderRight, TJointType.ElbowRight, skeleton.Joints);
                Plot(TJointType.ElbowRight, skeleton.Joints);
                Trace(TJointType.ElbowRight, TJointType.WristRight, skeleton.Joints);
                Plot(TJointType.WristRight, skeleton.Joints);
                Trace(TJointType.WristRight, TJointType.HandRight, skeleton.Joints);
                Plot(TJointType.HandRight, skeleton.Joints);

                if (!seated)
                {
                    Trace(TJointType.ShoulderCenter, TJointType.Spine, skeleton.Joints);
                    Plot(TJointType.Spine, skeleton.Joints);
                    Trace(TJointType.Spine, TJointType.HipCenter, skeleton.Joints);
                    Plot(TJointType.HipCenter, skeleton.Joints);

                    Trace(TJointType.HipCenter, TJointType.HipLeft, skeleton.Joints);
                    Plot(TJointType.HipLeft, skeleton.Joints);
                    Trace(TJointType.HipLeft, TJointType.KneeLeft, skeleton.Joints);
                    Plot(TJointType.KneeLeft, skeleton.Joints);
                    Trace(TJointType.KneeLeft, TJointType.AnkleLeft, skeleton.Joints);
                    Plot(TJointType.AnkleLeft, skeleton.Joints);
                    Trace(TJointType.AnkleLeft, TJointType.FootLeft, skeleton.Joints);
                    Plot(TJointType.FootLeft, skeleton.Joints);

                    Trace(TJointType.HipCenter, TJointType.HipRight, skeleton.Joints);
                    Plot(TJointType.HipRight, skeleton.Joints);
                    Trace(TJointType.HipRight, TJointType.KneeRight, skeleton.Joints);
                    Plot(TJointType.KneeRight, skeleton.Joints);
                    Trace(TJointType.KneeRight, TJointType.AnkleRight, skeleton.Joints);
                    Plot(TJointType.AnkleRight, skeleton.Joints);
                    Trace(TJointType.AnkleRight, TJointType.FootRight, skeleton.Joints);
                    Plot(TJointType.FootRight, skeleton.Joints);
                }
            }
        }
    }
}
