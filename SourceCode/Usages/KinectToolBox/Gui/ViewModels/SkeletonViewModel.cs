using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Kinect.Toolbox;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.UI;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Model;
using TKinect;

namespace Gui.ViewModels
{
    public class SkeletonViewModel
    {
        #region Attached properties

        public static readonly DependencyProperty KinectCanvasProperty = DependencyProperty.RegisterAttached("KinectCanvas", typeof(Canvas),
            typeof(SkeletonViewModel), new FrameworkPropertyMetadata(OnCanvasChanged));

        public static void SetKinectCanvas(DependencyObject element, Canvas value)
        {
            element.SetValue(KinectCanvasProperty, value);
        }

        public static Canvas GetKinectCanvas(DependencyObject element)
        {
            return (Canvas)element.GetValue(KinectCanvasProperty);
        }

        private static Canvas _kinectCanvas;
        private static SkeletonDisplayManager _skeletonDisplayManager;

        public static void OnCanvasChanged
        (DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            _kinectCanvas = obj as Canvas;
            AttachKinect();
        }

        private static void AttachKinect()
        {
            if (_skeletonDisplayManager != null)
            {
                return;
            }

            //var kinectSensor = DependencyFactory.Container.Resolve<KinectSensor>();
            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();

            var framesCollector = new FramesCollector(testKinect);
            _skeletonDisplayManager = new SkeletonDisplayManager(_kinectCanvas);
            framesCollector.FrameReady += (sender, arg) => _skeletonDisplayManager.Draw(arg.Frames.Last().Skeletons, false);
        }

        #endregion
    }
}
