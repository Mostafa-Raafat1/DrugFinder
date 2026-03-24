using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    public class Location : ValueObject
    {
        public NetTopologySuite.Geometries.Point Point { get; set; }

        private Location() { }
        public Location(double lat, double lng) 
        {
            var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            Point = factory.CreatePoint(new Coordinate(lng, lat));
        }

        public double LocationLat => Point.Y;
        public double LocationLng => Point.X;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationLat;
            yield return LocationLng;
        }
    }
}
