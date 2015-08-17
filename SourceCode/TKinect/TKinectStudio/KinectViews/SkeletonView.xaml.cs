using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;
using TKinect.Data.SkeletonData;
using TKinect.Display;

namespace TKinectStudio.KinectViews
{
    /// <summary>
    /// Interaction logic for SkeletonView.xaml
    /// </summary>
    public partial class SkeletonView : UserControl
    {
        public SkeletonView()
        {
            InitializeComponent();
        }

        private void SkeletonView_OnLoaded(object sender, RoutedEventArgs e)
        {
            SkeletonImage.Source = SkeletonDisplayHelper.ImageSource;
        }
    }
}
