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
    public class Location : ValueObject_
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


        // Converts degrees to radians
        private static double ToRadians(double angle) => angle * Math.PI / 180.0;

        // Calculates distance to another location in meters
        public double CalculateDistance(Location other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            const double R = 6371000; // Earth radius in meters

            double lat1Rad = ToRadians(this.LocationLat);
            double lat2Rad = ToRadians(other.LocationLat);
            double deltaLat = ToRadians(other.LocationLat - this.LocationLat); 
            double deltaLng = ToRadians(other.LocationLng - this.LocationLng);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLng / 2) * Math.Sin(deltaLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in meters
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LocationLat;
            yield return LocationLng;
        }

    }
}
