using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TKinect.Data.DepthData;
using TKinect.Display;

namespace TKinectStudio.KinectViews
{
    /// <summary>
    /// Interaction logic for DepthView.xaml
    /// </summary>
    public partial class DepthView : UserControl
    {
        public DepthView()
        {
            InitializeComponent();
        }

        private void DepthView_OnLoaded(object sender, RoutedEventArgs e)
        {
            DepthImage.Source = DepthDisplayHelper.DepthBitmap;
        }
    }
}
