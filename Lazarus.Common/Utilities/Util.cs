using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using System.Numerics;

namespace Lazarus.Common.Utilities
{
    public class Util
    {
        public static string GetExtenstionFile(string filename)
        {
            return Path.GetExtension(filename);
        }



        public  static string ToJson(object obj )
        {
           var s= JsonConvert.SerializeObject(obj, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
            return s;
        }

        public static bool CompareSerialization(string latest, string current)
        {
            var latestBigInt = BigInteger.Parse(latest);
            var currentBigInt = BigInteger.Parse(current);
            if (latestBigInt < currentBigInt)
                return true;

            return false;
        }

		public static bool CompareSerializationINTL(string latest, string current)
		{
			var latestBigInt = BigInteger.Parse(latest.Substring(0,14));
			var currentBigInt = BigInteger.Parse(current.Substring(0,14));

			if (latestBigInt < currentBigInt)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
