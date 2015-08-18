using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using TestStack.White;
using TestStack.White.Factory;
using TKinect;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;


namespace Tests
{
    [TestFixture]
    public class UITest
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void LeftHelloDetected()
        {
            
            Application application = Application.Launch(@"C:\Users\Styrna\Desktop\MGR\WORKSPACE\TKinect\Usages\GMap\Gui\bin\Debug\Gui.exe");

            Window window = application.GetWindow("MAIN_WINDOW", InitializeOption.NoCache);




            application.Close();
        }
    }
}
