//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Spatial;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lazarus.Common.Utilities
//{
//   public class GeoLocationHelper
//    {
//        public DbGeography CreatePoint(double lat, double lon, int srid = 4326)
//        {
//            try
//            {
//                var loc2 = DbGeography.FromText(string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", lon, lat));

//                string wkt = string.Format("POINT({0} {1})", lat.ToString(), lat.ToString());
//                var t = wkt;
//                return DbGeography.PointFromText(wkt, srid);
//            }
//            catch (Exception e)
//            {

//                throw e;
//            }

//        }
//    }
//}
