using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TKinect.Data.ColorData;
using TKinect.Display;

namespace TKinectStudio.KinectViews
{
    /// <summary>
    /// Interaction logic for ColorView.xaml
    /// </summary>
    public partial class ColorView : UserControl
    {
        public ColorView()
        {
            InitializeComponent();
        }

        private void ColorView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ColorImage.Source = ColorDisplayHelper.ColorBitmap;
        }
    }
}
