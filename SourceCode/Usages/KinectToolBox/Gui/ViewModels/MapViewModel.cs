using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Kinect.Toolbox;
using Kinect.Toolbox.Analyzers;
using Kinect.Toolbox.Collectors;
using Kinect.Toolbox.Extensions;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Model;
using Kinect.Toolbox.Utils;
using Gui.Controls;
using Console;
using Gui.CustomMarkers;
using TKinect;

namespace Gui.ViewModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private PostureAnalyzer _postureAnalyzer;
        private MovementAnalyzer _leftHandMovementAnalyzer;
        private ContinousMovementAnalyzer _headMovementAnalyzer;
        private ParallelSwipeAnalyzer _parallelSwipeAnalyzer;
        private HandInFrontAnalyzer _handNavigationAnalyzer;

        private bool CursorMarkerExists = false;
        private GMapMarker Marker {get;set;}

        private readonly IViewConfiguration _viewConfiguration;
        private readonly IMapDefinition _mapDefinition;

        public PointLatLng Position
        {
            get { return _mapDefinition.Position; }
            private set
            {
                GMapControl.Position = value;

                OnPropertyChanged("Position");
            }
        }

        public GMapControl GMapControl { get { return  DependencyFactory.Container.Resolve<GMapControl>(); } }

        public ObservableCollection<GMapMarker> Markers { get; private set; }

        public MapViewModel()
        {
            _viewConfiguration = DependencyFactory.Container.Resolve<IViewConfiguration>();
            _mapDefinition = DependencyFactory.Container.Resolve<IMapDefinition>();

            Markers = _mapDefinition.Markers;

            RegisterOnKinectEvents();

            Configuration.postureNavigation = true;
        }

        private void RegisterOnKinectEvents()
        {
            //var kinectSensor = DependencyFactory.Container.Resolve<KinectSensor>();
            var testKinect = DependencyFactory.Container.Resolve<TKinect.TKinect>();
            var framesCollector = new FramesCollector(testKinect, 30);

            _postureAnalyzer = new PostureAnalyzer(framesCollector, 25);
            _postureAnalyzer.PostureDetected += PostureAnalyzerOnPostureDetected;

            var stabilityFramesCollector = new FramesCollector(testKinect, 40);

            var HandCollector = new FramesCollector(testKinect, 10);
            var trafficReduceExtension = new TrafficReduceExtension(0);

            _handNavigationAnalyzer = new HandInFrontAnalyzer(HandCollector);
            _handNavigationAnalyzer.AddBeforeAnalyzeExtension(trafficReduceExtension);
            _handNavigationAnalyzer.MovementDetected += MovementAnalyzerOnOnMovementGestureDetected;

            
        }

        private void PostureAnalyzerOnPostureDetected(object sender, PostureEventArgs args)
        {
            switch (args.Posture)
            {
                case PosturesEnum.LeftHello:
                    {
                        PointLatLng pos;
                        if(Marker != null)
                        {
                            pos = new PointLatLng(Marker.Position.Lat, Marker.Position.Lng);
                        }
                        else
                        {
                            pos = new PointLatLng(GMapControl.Position.Lat, GMapControl.Position.Lng);
                        }

                        GMapMarker flag = new GMapMarker(new PointLatLng())
                        {
                            Position = pos,
                            ZIndex = int.MaxValue
                        };
                        flag.Shape = new CustomMarkerDemo(flag, "FLAG");
                        GMapControl.Markers.Add(flag);
                    }
                    break;

                case PosturesEnum.RightHandOverHead:
                    {
                        Configuration.maxHandLength = 0.4;
                        Configuration.activeHandDistance = 0.25;
                        LogString.Log("Restoring defaults");
                    }
                    break;

                case PosturesEnum.LeftHandOverHead:
                    {
                        LogString.Log("LeftOverHead");
                        if (GMapControl.Markers.Any())
                        {
                            var marker = GMapControl.Markers.Last();
                            marker.Shape = null;
                            GMapControl.Markers.Remove(marker);
                        }
                    }
                    break;


                case PosturesEnum.HandsJoined:
                    {
                        CursorMarkerExists = false;
                        if (Marker != null)
                        {
                            GMapControl.Markers.Remove(Marker);
                            Marker = null;
                            //LogString.Log(Marker.ToString());
                        }


                        if (GMapControl.Markers.Count == 0)
                        {
                            return;
                        }
                        var mRoute = new GMapMarker(GMapControl.Markers[0].Position);
                        {
                            mRoute.Route.AddRange(GMapControl.Markers.Select(a => new PointLatLng(a.Position.Lat - 100, a.Position.Lng)));
                            var gMapControl = GMapControl.Markers[0].Map;
                            mRoute.RegenerateRouteShape(gMapControl);
                            mRoute.ZIndex = -1;
                        }
                        GMapControl.Markers.Add(mRoute);
                    }
                    break;
            }
        }

        private void MovementAnalyzerOnOnMovementGestureDetected(object sender, MovementEventArgs movementEventArgs)
        {
            var milis = DateTime.Now.Ticks;

            if (movementEventArgs.ControlType == ControlType.LEFT_HAND)
            {
                CursorMarkerExists = false;
                if (Marker != null)
                {
                    GMapControl.Markers.Remove(Marker);
                    Marker = null;
                    //LogString.Log(Marker.ToString());
                }
                var amplification = 0.1;
                amplification = (GMapControl.ViewArea.LocationTopLeft.Lat - GMapControl.ViewArea.LocationRightBottom.Lat);
                //LogString.Log("MAP_MOVE:[" + movementEventArgs.Displacement.ToString()+"]");
                var displacement = Math.Sqrt(movementEventArgs.Displacement.Y * movementEventArgs.Displacement.Y + movementEventArgs.Displacement.X * movementEventArgs.Displacement.X);
                
                if (displacement > 0.008)
                {
                    GMapControl.Position = new PointLatLng(GMapControl.Position.Lat - movementEventArgs.Displacement.Y * amplification, GMapControl.Position.Lng - movementEventArgs.Displacement.X * amplification);
                }
                
            }
            else if (movementEventArgs.ControlType == ControlType.BOTH_HANDS)
            {
                //LogString.Log("ZOOM:["+movementEventArgs.Displacement.X+"]");
                CursorMarkerExists = false;
                if (Marker != null)
                {
                    GMapControl.Markers.Remove(Marker);
                    Marker = null;
                    //LogString.Log(Marker.ToString());
                }
                var zoomAmplifier = 2;

                GMapControl.Zoom -= movementEventArgs.Displacement.X * zoomAmplifier;
 
            }
            else if (movementEventArgs.ControlType == ControlType.RIGHT_HAND)
            {
                
                if (!CursorMarkerExists)
                {
                    Marker = new GMapMarker(new PointLatLng())
                    {
                        Position = new PointLatLng(GMapControl.Position.Lat, GMapControl.Position.Lng),
                        ZIndex = int.MaxValue,
                        Shape = new CustomMarkerRed(Marker,"Cursor")
                    };
                    GMapControl.Markers.Add(Marker);

                    CursorMarkerExists = true;
                }
                //LogString.Log("CURSOR_MOVE:[" + movementEventArgs.Displacement.ToString() + "]");

                var amplification = 0.1;
                //var amplification = GMapControl.Zoom / 50;
                PointLatLng pos = Marker.Position;

                amplification = (GMapControl.ViewArea.LocationTopLeft.Lat - GMapControl.ViewArea.LocationRightBottom.Lat) ;

                if ((pos.Lat - movementEventArgs.Displacement.Y * amplification < GMapControl.ViewArea.LocationTopLeft.Lat) &&
                    (pos.Lat - movementEventArgs.Displacement.Y * amplification > GMapControl.ViewArea.LocationRightBottom.Lat))
                {
                    pos.Lat -= movementEventArgs.Displacement.Y * amplification;
                }
                if ((pos.Lng - movementEventArgs.Displacement.X * amplification > GMapControl.ViewArea.LocationTopLeft.Lng) &&
                    (pos.Lng - movementEventArgs.Displacement.X * amplification < GMapControl.ViewArea.LocationRightBottom.Lng))
                {
                    pos.Lng -= movementEventArgs.Displacement.X * amplification;
                }

                Marker.Position = pos;
            }
            var passed = DateTime.Now.Ticks - milis;
            if (passed > -1)
            {
                //LogString.Log(this.GetType().Name + " took: " + passed);
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
