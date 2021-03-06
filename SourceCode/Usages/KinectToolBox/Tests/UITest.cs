﻿using System;
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
        private const string RecordingPath = @"C:\Users\Styrna\Desktop\PassedData\hello.xed";

        private TKinect.TKinect Kinect { get; set; }

        [SetUp]
        public void Setup()
        {
            Kinect = new TKinect.TKinect();
            Kinect.SkeletonFrameHost.Start(4503);
        }

        [Test]
        public void LeftHelloDetected()
        {
            //Arrange
            Application application = Application.Launch(@"..\..\..\HelloKinectWPF\bin\Debug\HelloKinectWPF.exe");
            Window window = application.GetWindow("MainWindow", InitializeOption.NoCache);
            window.WaitWhileBusy();

            //Act
            var fileStream = new FileStream(RecordingPath, FileMode.Open);
            Kinect.ReplayStart(fileStream);
            Thread.Sleep(8000);

            //Assert
            var label = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("Label"));
            Assert.That(label.Text == "Hello Kinect");

            fileStream.Close();
            application.Close();
        }
    }
}
