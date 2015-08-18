using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kinect.Toolbox;
using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Model;
using Kinect.Toolbox.Utils;
using TKinect;

namespace Gui.ViewModels
{
    public class ControlPanelViewModel : INotifyPropertyChanged
    {
        private readonly PostureAnalyzer _algorithmicPostureDetector;

        public bool IsHandNavigationMode
        {
            get { return Configuration.handNavigation; }
            private set
            {
                OnPropertyChanged("IsHandNavigationMode");
                Console.LogString.Log("Hand Navigation: " + value);
                Configuration.handNavigation = value;
            }
        }

        public bool IsHeadNavigationMode
        {
            get { return Configuration.headNavigation; }
            private set
            {
                OnPropertyChanged("IsHeadNavigationMode");
                Console.LogString.Log("Head Navigation: " + value);
                Configuration.headNavigation = value;
            }
        }

        public bool IsSwipeNavigationMode
        {
            get { return Configuration.swipeNavigation; }
            private set
            {
                OnPropertyChanged("IsSwipeNavigationMode");
                Console.LogString.Log("Swipe Navigation: " + value);
                Configuration.swipeNavigation = value;
            }
        }

        public bool IsDualSwipeNavigationMode
        {
            get { return Configuration.dualSwipeNavigation; }
            private set
            {
                OnPropertyChanged("IsDualSwipeNavigationMode");
                Console.LogString.Log("Dual Swipe Navigation: " + value);
                Configuration.dualSwipeNavigation = value;
            }
        }

        public bool IsPostureNavigationMode
        {
            get { return Configuration.postureNavigation; }
            private set
            {
                OnPropertyChanged("IsPostureNavigationMode");
                Console.LogString.Log("Posture Navigation: " + value);
                Configuration.postureNavigation = value;
            }
        }

        public bool IsContinousNavigationMode
        {
            get { return Configuration.movementNavigation; }
            private set
            {
                OnPropertyChanged("IsContinousNavigationMode");
                Console.LogString.Log("Continous Navigation: " + value);
                Configuration.movementNavigation = value;
            }
        }

        public ControlPanelViewModel()
        {
            //var kinectSensor = DependencyFactory.Container.Resolve<KinectSensor>();

            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();
            var framesCollector = new FramesCollector(testKinect);
            //_algorithmicPostureDetector = new PostureAnalyzer(framesCollector);
            //_algorithmicPostureDetector.PostureDetected += AlgorithmicPostureDetectorOnPostureDetected;
        }

        private void AlgorithmicPostureDetectorOnPostureDetected(object sender, PostureEventArgs args)
        {
            switch (args.Posture)
            {
                case PosturesEnum.LeftHandOverHead:
                    {
                        //IsHandNavigationMode = !IsHandNavigationMode;
                    }
                    break;
                case PosturesEnum.LeftHello:
                    {
                        //IsHeadNavigationMode = !IsHeadNavigationMode;
                    }
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
