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
using TKinect.Data.DepthData;
using TKinect.Display;

namespace Depth
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
            DepthDisplayHelper.Init();
            Tkinect.DepthFrameReady += DepthDisplayHelper.SensorDepthFrameReady;
            Image.Source = DepthDisplayHelper.DepthBitmap;

            //To connect to Studio
            Tkinect.DepthFrameClient.Connect("localhost", 4502);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != Tkinect)
            {
                Tkinect.DepthFrameClient.Disconnect();
            }
        }
    }
}