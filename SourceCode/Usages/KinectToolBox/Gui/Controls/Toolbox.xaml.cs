using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Model;
using TKinect;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace Gui.Controls
{
    /// <summary>
    /// Interaction logic for Toolbox.xaml
    /// </summary>
    public partial class Toolbox : UserControl
    {
        public Toolbox()
        {
            InitializeComponent();
        }

        private FileStream replayStream;

        private void RecordClicked(object sender, RoutedEventArgs e)
        {
            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();
            var recordTask = new Task(() =>
            {
                Thread.Sleep(1000);

                using (var fileStream = new FileStream(@"RECORDING.xed", FileMode.Create))
                {
                    //testKinect.Re(KinectRecordOptions.Color | KinectRecordOptions.Depth | KinectRecordOptions.Skeletons, fileStream);
                    
                    //Thread.Sleep(3000);
                    
                    //testKinect.Stop();
                }
            });
            recordTask.Start();
        }

        private void ReplayClicked(object sender, RoutedEventArgs e)
        {
            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();
            if (replayStream != null)
            {
                replayStream.Dispose();
            }

            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.DefaultExt = ".xed";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = fd.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                //replayStream = new FileStream(fd.FileName, FileMode.Open);
                //testKinect.Replay(replayStream);

            }


        }

       
    }
}
