using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;

namespace Model
{
    public static class Utils
    {
        public static PointLatLng GetPositionByKeywords(string keys)
        {
            GeoCoderStatusCode status;
            GeocodingProvider geocodingProvider = GMapProviders.GoogleMap;
            if (geocodingProvider != null)
            {
                PointLatLng? point = geocodingProvider.GetPoint(keys, out status);
                if (status == GeoCoderStatusCode.G_GEO_SUCCESS && point.HasValue)
                    return point.Value;
            }
            return new PointLatLng();
        }
    }
}
