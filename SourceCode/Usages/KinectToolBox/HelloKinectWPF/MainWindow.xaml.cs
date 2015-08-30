using System;
using System.Collections.Generic;
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

namespace HelloKinectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Initialized(object sender, EventArgs e)
    {
        var hello = new HelloKinect.HelloKinect();
        hello.HelloDetected += hello_HelloDetected;


        //hello.RunRealKinect();
        //hello.RunKinectReplay();
        hello.RunKinectClient();
    }

    private void hello_HelloDetected(object sender, int helloCount)
    {
        this.Dispatcher.Invoke(() =>
        {
            Label.Content = "Hello Kinect";
        });
    }
}
}
