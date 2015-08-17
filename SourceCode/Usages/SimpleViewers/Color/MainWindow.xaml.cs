using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using TKinect.Data.ColorData;
using TKinect.Display;

namespace Color
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TKinect.TKinect Tkinect;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Tkinect = new TKinect.TKinect();
            ColorDisplayHelper.Init();
            Tkinect.ColorFrameReady += ColorDisplayHelper.SensorColorFrameReady;
            Image.Source = ColorDisplayHelper.ColorBitmap;

            //To connect to Studio
            Tkinect.ColorFrameClient.Connect("localhost", 4501);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != Tkinect)
            {
                Tkinect.ColorFrameClient.Disconnect();
            }
        }
    }
}