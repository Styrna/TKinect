using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace Model
{
    public interface IMapDefinition
    {
        PointLatLng Position { get; set; }
        ObservableCollection<GMapMarker> Markers { get; }
    }

    public class MapDefinition : IMapDefinition
    {
        public PointLatLng Position { get; set; }
        public ObservableCollection<GMapMarker> Markers { get; private set; }

        public MapDefinition()
        {
            Markers = new ObservableCollection<GMapMarker>();
        }
    }
}
