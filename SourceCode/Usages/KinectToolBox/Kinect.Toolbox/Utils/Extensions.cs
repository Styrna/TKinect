using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;
using TKinect.Data;

namespace Kinect.Toolbox.Utils
{
    public static class Extensions
    {


        public static Vector3 ToVector3(this TSkeletonPoint vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
        public static Vector2 ToVector2(this TSkeletonPoint vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static TSkeleton GetNearestSkeleton(this Frame frame)
        {
            var trackedSkeletons = frame.Skeletons.Where(s => s.TrackingState != TSkeletonTrackingState.NotTracked);
            if (!trackedSkeletons.Any())
            {
                return frame.Skeletons[0];
            }
            var minimumDistance = trackedSkeletons.Min(a => a.Position.Z);
            var skeleton = frame.Skeletons.FirstOrDefault(s => Math.Abs(s.Position.Z - minimumDistance) < 0.001f);
            return skeleton ?? frame.Skeletons[0];
        }

        public static Joint GetJoint(this Skeleton skeleton, JointType joint)
        {
            return skeleton.Joints.FirstOrDefault(a => a.JointType == joint);
        }







        #region Print

        public static string Print(SkeletonPoint pos)
        {
            return string.Format("[{0}:{1}:{2}]",pos.X,pos.Y,pos.Z);
        }

        #endregion
    }
}
