using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Kinect.Toolbox;
using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Model;
using TKinect;

namespace Gui.ViewModels
{
    public class ToolboxViewModel : INotifyPropertyChanged
    {
        private readonly SwipeAnalyzer _handSwipeAnalyzer;
        private int _selectedIndex;
        private readonly PostureAnalyzer _postureAnalyzer;
        private readonly IMapDefinition _mapDefinition;
        private readonly FramesCollector _framesCollector;
        private TKinect.TKinect kinect;
        private FileStream replayStream;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            private set
            {
                _selectedIndex = value;
                if(_selectedIndex < 0)
                {
                    _selectedIndex++;
                }
                if (_selectedIndex == Cities.Count)
                {
                    _selectedIndex--;
                }

                OnPropertyChanged("SelectedIndex");
            }
        }

        public List<string> Cities { get; private set; }

        public ToolboxViewModel()
        {

            _mapDefinition = DependencyFactory.Container.Resolve<IMapDefinition>();

            Cities = new List<string>() {"Record", "Replay"};

            //var kinectSensor = DependencyFactory.Container.Resolve<KinectSensor>();
            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();
            _framesCollector = new FramesCollector(testKinect);

            //_postureAnalyzer = new PostureAnalyzer(_framesCollector);
            //_postureAnalyzer.PostureDetected += PostureAnalyzerOnPostureDetected;

            //_handSwipeAnalyzer = new SwipeAnalyzer(_framesCollector, JointType.HandRight);
            //_handSwipeAnalyzer.SwipeDetected  += HandSwipeAnalyzerOnSwipeDetected;
        }

        private void HandSwipeAnalyzerOnSwipeDetected(object sender, SwipeEventArgs args)
        {
            if (args.Gesture == GesturesEnum.SwipeToLeft)
            {
                SelectedIndex--;
            }
            else if (args.Gesture == GesturesEnum.SwipeToRight)
            {
                SelectedIndex++;
            }
        }

        private void PostureAnalyzerOnPostureDetected(object sender, PostureEventArgs args)
        {
            switch (args.Posture)
            {
                case PosturesEnum.RightHandOverHead:
                    {
                        var city = Cities[SelectedIndex];
                        _mapDefinition.Markers.Add(new GMapMarker(Utils.GetPositionByKeywords(city))
                        {
                            ZIndex = int.MaxValue,
                            Shape =
                                new Label()
                                {
                                    Width = 100,
                                    Content = city,
                                    Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255))
                                }
                        });
                    }
                    break;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
