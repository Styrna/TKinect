using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Microsoft.Kinect;
using Model;
using Microsoft.Practices.Unity;

namespace Gui.Controls
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map
    {
        public Map()
        {
            InitializeComponent();
            //GMapControl.ShowTileGridLines = true;
            GMapControl.ShowCenter = true;

            GMapControl.MapProvider = GMapProviders.GoogleMap;

            //GMapControl.SetCurrentPositionByKeywords("Kraków");

            //GMapControl.ScaleMode = ScaleModes.Dynamic;
            GMapControl.Zoom = 13;

            DependencyFactory.Container.RegisterInstance(typeof(GMapControl), "", GMapControl, new ContainerControlledLifetimeManager());
        }

    
    }
}
