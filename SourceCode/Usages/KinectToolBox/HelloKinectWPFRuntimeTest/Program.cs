using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TKinect;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using System.IO;
using System.Threading;

namespace HelloKinectWPFRuntimeTest
{
    class Program
    {
        private const string RecordingPath = @"C:\Users\Styrna\Desktop\PassedData\hello.xed";

        static void Main(string[] args)
        {
            var tkinect = new TKinect.TKinect();
            tkinect.SkeletonFrameHost.Start(4503);

            Application application = Application.Launch(@"..\..\..\HelloKinectWPF\bin\Debug\HelloKinectWPF.exe");
            Window window = application.GetWindow("MainWindow", InitializeOption.NoCache);
            window.WaitWhileBusy();

            var label = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("Label"));
            

            var fileStream = new FileStream(RecordingPath, FileMode.Open);
            tkinect.ReplayStart(fileStream);
            Thread.Sleep(8000);

            var label2 = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("Label"));

            fileStream.Close();
            application.Close();
        }
    }
}
