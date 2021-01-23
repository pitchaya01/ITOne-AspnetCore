using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;
using Lazarus.Common.Domain;
using Lazarus.Common.Enum;
using Lazarus.Common.Model;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Core.ExcelPackage;

using static Lazarus.Common.Attributes.ReadExcelAttribute;

namespace Lazarus.Common.Utilities
{


    public static class Extension
    {
        public static string GetErrorHtml( this ModelState modelState)
        {

            return "";
        }
        public static string GetExtension(this string filename)
        {
            var result = System.IO.Path.GetExtension(filename);
            return result;
        }
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            return input;
        }
        public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
        {
            if (pageSize == 0)
                pageSize = 10;
            if (page == 0)
                page = 1;

            page = page - 1;
            return en.Skip(page * pageSize).Take(pageSize);
        }

        public static bool IsNullOrEmpty(this string param)
        {
            return string.IsNullOrEmpty(param);
        }

        public static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(newName);
        }
        //public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
        //{
        //    page = page - 1;
        //    return en.Skip(page * pageSize).Take(pageSize);
        //}

        public static bool IsNotNullOrZero(int? v)
        {
            return (v.GetValueOrDefault() != 0);
        }

        public static List<string> SplitCommaToArray(this string s)
        {
            if (string.IsNullOrEmpty(s)) return new List<string>();
            return s.Split(',').Where(a => string.IsNullOrEmpty(a) == false).Distinct().ToList();
        }
 
        public static List<string> SplitSemicolonToList(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new List<string>();
            var s = str.Split(';').Where(x => string.IsNullOrEmpty(x) == false).ToList();
            return s;
        }
        public static bool IsHasValue(this List<string> lstr)
        {
            if (lstr == null || lstr.Any() == false)
                return false;
            var isHasvalue = false;
            lstr.ForEach(x =>
            {
                if (string.IsNullOrEmpty(x) == false)
                    isHasvalue = true;
            });
            return isHasvalue;
        }
        public static bool IsNotNullOrZero(decimal? v)
        {
            return (v.GetValueOrDefault() != 0);
        }

        public static string IntToMonthName(this int monthNumber)
        {
            var month = new List<string>()
            {
                "มกราคม",
                "กุมภาพันธ์",
                "มีนาคม",
                "เมษายน",
                "พฤษภาคม",
                "มิถุนายน",
                "กรกฏาคม",
                "สิงหาคม",
                "กันยายน",
                "ตุลาคม",
                "พฤษจิกายน",
                "ธันวาคม"
            };
            return month[monthNumber - 1].ToString();
        }

        public static int? StringToInt(this string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            return int.Parse(s);
        }
        public static int ToInt(this string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            return Convert.ToInt32(Math.Floor(Convert.ToDouble(s)));
        }
        public static double ToDouble(this string s)
        {

            return double.Parse(s);
        }

        public static string ToCurrency(this double d)
        {

            return d.ToString("n0");
        }
        public static string ToCurrency(this int d)
        {

            return d.ToString("n0");
        }
        public static string NullToEmpty(this string s)
        {
            if (s == null) return "";

            return s;
        }
        public static String nameof<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            if (propertyAccessor.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = propertyAccessor.Body as MemberExpression;
                if (memberExpression == null)
                    return null;
                return memberExpression.Member.Name;
            }
            return null;
        }

        public static double RoundDown(this double num)
        {
            return Math.Round(num);

        }
        public static T ToEnum<T>(this string value)
        {

            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        public enum SizeUnits
        {
            Byte, KB, MB, GB, TB, PB, EB, ZB, YB
        }

        public static double ToSize(this int value, SizeUnits unit)
        {
            return (value / (double)Math.Pow(1024, (Int64)unit));
        }
        public static string ToEmpty(this string s)
        {
            if (s == null) return "";
            return s;
        }
        public static string ByteArrayToString(this byte[] ba)
        {
            if (ba == null) return "";
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return "0x" + hex.ToString();
        }
        public static string WordWrap(this string the_string, int width)
        {
            if (string.IsNullOrEmpty(the_string)) return string.Empty;
            if (the_string.Length > width) return the_string.Remove(width) + "...";

            return the_string;

        }
        public static bool IntToIsActive(this int isactive)
        {
            if (isactive == 1)
                return true;

            return false;
        }
        public static List<T> Fill<T>(this DbDataReader reader) where T : new()
        {
            List<T> res = new List<T>();
            while (reader.Read())
            {
                T t = new T();
                for (int inc = 0; inc < reader.FieldCount; inc++)
                {
                    Type type = t.GetType();
                    string name = reader.GetName(inc);
                    PropertyInfo prop = type.GetProperty(name);
                    if (prop != null)
                    {
                        if (name == prop.Name)
                        {
                            var value = reader.GetValue(inc);
                            if (value != DBNull.Value)
                            {
                                prop.SetValue(t, Convert.ChangeType(value, prop.PropertyType), null);
                            }
                            //prop.SetValue(t, value, null);

                        }
                    }
                }
                res.Add(t);
            }
            reader.Close();

            return res;
        }
        public static Guid ToGuid(this string s)
        {
            if (string.IsNullOrEmpty(s)) return new Guid();
            return new Guid(s);
        }
       public static bool IsValidEmail( this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string ToImage(this string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl)) return imageUrl;
            return AppConfigUtilities.GetAppConfig<string>("No_IMAGE");
        }
        public static string GetDateStringYYYYMMDD_HHmm(this DateTime datetime)
        {
            return datetime.ToString("yyyyMMdd_HHmm");
        }
        public static string GetDateStrignDDMMMYYYHHMM(this DateTime? datetime)
        {
            return datetime.Value.ToString("dd MMM yyyy HH:mm");
        }
        public static string GetDateStringddMMyyyyHHmmss(this DateTime? datetime)
        {
            if (!datetime.HasValue)            
                return "NULL";
            
            return datetime.Value.ToString("ddMMyyyy HH:mm:ss");
        }
        public static string GetDateStringddMMyyyyHHmmssWithLang(this DateTime? datetime)
        {
            if (!datetime.HasValue)
                return null;

            var culture = AppConfigUtilities.CreateCulture("en");
            return datetime.Value.ToString("ddMMyyyy HH:mm:ss", culture);
        }
        public static string GetDateStringForSQLParam(this DateTime datetime)
        {     
            var culture = AppConfigUtilities.CreateCulture("en");
            return datetime.ToString("yyyy-MM-dd HH:mm:ss", culture);
        }
        public static string GetDateStrignDDMMMYYYHHMM(this DateTime datetime)
        {
            return datetime.ToString("dd MMM yyyy HH:mm");
        }
        public static string GetDateStrignDDMMYYYYHHMMSS(this DateTime datetime)
        {
            return datetime.ToString("dd MM yyyy HH:mm:ss");
        }
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static long ConvertToTimestamp(this DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
        public static string GetDateStringYYYYMMDD(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }
        public static string GetTimeStringHHmm(this DateTime? datetime)
        {
            return datetime?.ToString("HH:mm");
        }
        public static string GetTimeStringHHmmss(this DateTime datetime)
		{
			return datetime.ToString("HH:mm:ss");
		}
		public static string GetDateMonth(this DateTime datetime)
        {
            return datetime.ToString("MMMM yyyy");
        }
        public static string GetDateStrignDDMMMYYY(this DateTime datetime)
        {

            return datetime.ToString("d MMM yy");
        }
        public static string GetDateStrignDDMMMMYYY(this DateTime datetime)
        {

            return datetime.ToString("dd MMMM yy");
        }
        public static string GetDateStrignDDMMMYY(this DateTime datetime, EnumLang lang)
        {
            var culture = AppConfigUtilities.CreateCulture(lang.ToString());
            return datetime.ToString("d MMM yy", culture);
        }
        public static string GetDateStrignDDMMYYYY(this DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy");
        }
        public static string GetDateStringDotDDMMYYYY(this DateTime datetime)
        {
            return datetime.ToString("dd.MM.yyyy");
        }
        public static string GetDateStringDotDDMMYYYY(this DateTime? datetime)
        {
            return datetime?.ToString("dd.MM.yyyy");
        }
        public static string GetDateTimeStringDotDDMMYYYY(this DateTime? datetime)
        {
            return datetime?.ToString("dd.MM.yyyy HH:mm:ss");
        }
        public static string GetFullDateTimeString(this DateTime dataTime)
        {
            return dataTime.ToString("dd/MM/yyyy HH:mm");
        }
        public static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                var result = new StringBuilder();

                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }

                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = GetFriendlyName(typeParameters[i]);
                    result.Append(typeParamName);


                }
                result.Append(friendlyName);
                return result.ToString();
            }

            return friendlyName;
        }
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        public static string GetCSV<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
        public static string GetDateString(this DateTime dataTime)
        {
            return dataTime.ToLocalTime().ToString("dd/MM/yyyy hh:mm");
        }


        public static DateTime ToLocalDate(this DateTime dateTime)
        {            
            dateTime = dateTime.ToUniversalTime();
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(dateTime, tzi);
        }

        public static string ToStringNullZero(this string s)
        {
            if (s == null) return "0";

            return s;
        }
        public static string ToStringNull(this string s)
        {
            if (s == null) return "";

            return s;
        }

        public static bool AnyAndNotNull<T>(this IEnumerable<T> l)
        {
            if (l == null) return false;
            if (l.Any() == false) return false;
            return true;
        }
 
        public static string ToJSON(this object obj)
        {
            if (obj == null) return "";
            return JsonConvert.SerializeObject(obj);     
        }
        public static string ToJSONRemoveNull(this object obj)
        {
            return JsonConvert.SerializeObject(obj,
                      Formatting.None,
                      new JsonSerializerSettings
                      {
                          NullValueHandling = NullValueHandling.Ignore
                      });
        }
        public static string ListToString(this List<string> strs)
        {
            if (strs.AnyAndNotNull() == false) return "";
            var r = String.Join(",", strs);
            return r;
        }
        public static string ListToHtmlString(this List<string> strs)
        {
            var str = new StringBuilder();
            str.Append("<ul>");
            if (strs.AnyAndNotNull())
            {
                strs.ForEach(a =>
                {
                    str.Append($"<li>{a}</li>");
                });
            }
            else {
                return "";
            }
            str.Append("</ul>");
             
            return str.ToString();
        }
        public static float ToSizeMb(this byte[] files)
        {
            return (files.Length / 1024f) / 1024f;
        }
        public static decimal RoundUp(this decimal input, int places)
        {
            decimal multiplier = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(places)));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        public static double RoundUp(this double figure, int precision)
        {
            return Math.Ceiling(figure * Math.Pow(10, precision)) / Math.Pow(10, precision);
        }

 
      
   
        public static byte[] ReadFully(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static string ToJSON(this object obj, int recursionDepth)
        {
            var jsonStr = JsonConvert.SerializeObject(obj);
            return jsonStr;
        }

        public static string CurrencyToIcon(this string str)
        {
            if (str.ToLower() == "baht")
                return " ฿";

            return "$";
        }
        public static T ToObject<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception e)
            {
                var ex = new Exception("can't deserializeObject object:" + s);
                throw ex;
            }

        }
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
        public static T NullOrValue<T>(this T val)
        {
            if(val==null)
            return  default(T);

            return val;
        }
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public static TimeSpan StringHHMMToTime(this string time)
        {
            return TimeSpan.Parse(time);
        }
        public static DateTime StringToTime2(this string time)
        {
            var h = int.Parse(time.Split('.')[0]);
            var sec = int.Parse(time.Split('.')[1]);
            var stWork = DateTime.Now.ToLocalDate().Date.AddHours(h).AddMinutes(sec);
            return stWork;
        }
        public static DateTime AddTime(this DateTime date, string time)
        {
            var h = int.Parse(time.Split(':')[0]);
            var sec = int.Parse(time.Split(':')[1]);

            var stWork = date.Date.AddHours(h).AddMinutes(sec);
            return stWork;
        }

        public static string ToTime(this DateTime dt)
        {
            return dt.ToString("HH:mm");
        }
        public static DateTime AddTimeStringToDatetime(this string time, DateTime date)
        {
            var ss = time.Split(':')[0];
            var h = int.Parse(ss);
            var sec = int.Parse(time.Split(':')[1]);
            var stWork = date.Date.AddHours(h).AddMinutes(sec);
            return stWork;
        }
        public static string GetMessageError(this Exception e)
        {
            while (e.InnerException != null) e = e.InnerException;
            return e.Message;
        }
        public static DateTime StringToDateTime(this string sDate)
        {

            try
            {
                DateTime date = DateTime.ParseExact(sDate, "dd/MM/yyyy", null);
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                return TimeZoneInfo.ConvertTime(date, tzi);
            }
            catch (Exception)
            {
                try
                {
                    DateTime date = DateTime.ParseExact(sDate, "dd-MM-yyyy", null);
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    return TimeZoneInfo.ConvertTime(date, tzi);
                }
                catch (Exception)
                {

                    try
                    {
                        DateTime date = DateTime.ParseExact(sDate, "yyyy-MM-dd", null);
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                        return TimeZoneInfo.ConvertTime(date, tzi);
                    }
                    catch (Exception)
                    {

                        try
                        {
                            DateTime date = DateTime.Parse(sDate);
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                            return TimeZoneInfo.ConvertTime(date, tzi);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
                throw;
            }

        }

        public static DateTime StringWithTimeToDateTime(this string sDate, string sTime = "")
        {
            try
            {
                //DateTime date;
                //var result = DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                //if (result == false)
                //{
                //    throw new InvalidCastException("Date time(" + dateString + ") is invalid format(" + format + ").");
                //}

                var date = new DateTime();
                if (!string.IsNullOrEmpty(sTime))
                {
                    var result = DateTime.TryParseExact($"{sDate} {sTime}", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    if (result == false)
                        throw new InvalidCastException($"Date time({sDate}) is invalid format('dd/MM/yyyy HH:mm').");

                    //date = DateTime.ParseExact($"{sDate} {sTime}", "dd/MM/yyyy HH:mm", null);
                }
                else
                {
                    var result = DateTime.TryParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    if (result == false)
                        throw new InvalidCastException($"Date time({sDate}) is invalid format('dd/MM/yyyy').");

                    //date = DateTime.ParseExact(sDate, "dd/MM/yyyy", null);
                }

                return date;
                //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                //return TimeZoneInfo.ConvertTime(date, tzi);
            }
            catch (Exception)
            {              
                throw;
            }
        }

        public static DateTime? StringToDateTimeddMMMyy(this string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) return null;
            DateTime date = DateTime.ParseExact(sDate, "d MMM yy", null);



            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(date, tzi);

        }

        public static string TimeToString(this TimeSpan t)
        {
            var result = string.Format("{0}:{1}", t.Hours.ToString().PadLeft(2, '0'),
                t.Minutes.ToString().PadLeft(2, '0'));
            return result;
        }
        public static DateTime StringToDateTimedMMYYYY(this string sDate)
        {
            DateTime date = DateTime.ParseExact(sDate, "d/MM/yyyy", null);

            //var date = DateTime.Parse(sDate);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(date, tzi);
        }

		public static DateTime? StringToDateTimedYYYYMMDD(this string sDate)
		{
			if (string.IsNullOrEmpty(sDate))
			{
				return null;
			}
			DateTime date = DateTime.ParseExact(sDate, "yyyyMMdd", null);

			//var date = DateTime.Parse(sDate);

			TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
			return TimeZoneInfo.ConvertTime(date, tzi);
		}

        public static bool TryParseStringToDate(this string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) return false;

            if (!DateTime.TryParseExact(sDate, "dd/MM/yyyy",
                       CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                return false;

            return true;
        }

        public static bool TryParseStringToDateTime(this string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) return false;

            if (!DateTime.TryParseExact(sDate, "ddMMyyyy HH:mm:ss",
                       CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                return false;

            return true;
        }

        public static bool TryParseStringToTime(this string sTime)
        {
            if (string.IsNullOrEmpty(sTime)) return false;
            if (!DateTime.TryParseExact(sTime, "HH:mm",
                       CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return false;

            return true;
        }

        public static string CompareEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str)) return "Empty";

            return str;
        }
		public static bool TryParseStringToDateTimeYYYYMMDD(this string sDate)
		{
			if (!DateTime.TryParseExact(sDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
			{
				return false;
			}

			return true;
		}
		public static DateTime? StringDateTimeToDateTimeDDMMYYYY(this string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) return null;
            DateTime date;
            DateTime.TryParseExact(sDate, "ddMMyyyy HH:mm:ss", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            //var date = DateTime.Parse(sDate);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(date, tzi);
        }
        public static DateTime? StringToDateTimeDDMMYYYY(this string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) return null;
            DateTime date = DateTime.ParseExact(sDate, "dd/MM/yyyy", null);

            //var date = DateTime.Parse(sDate);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(date, tzi);
        }        
        public static DateTime? StringToDateTimeDDMMMYYYY(this string sDate)
        {
            try
            {
                if (string.IsNullOrEmpty(sDate)) return null;
                DateTime date = DateTime.ParseExact(sDate, "dd MMMM yy", null);

                //var date = DateTime.Parse(sDate);

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                return TimeZoneInfo.ConvertTime(date, tzi);
            }
            catch (Exception)
            {

                throw new MessageError("invalid datetime");
            }

        }
        public static string DateTimeToString(this DateTime date)
        {

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTime(date, tzi).ToString("dd/MM/yyyy");
            return date.ToString("dd/MM/yyyy");
        }

        public static T DeepClone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }       
      
        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

		public static string AsNullIfEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str) ? str : null;
		}

        //public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : new()
        //{
        //    bool columnOnly(CustomAttributeData y) => y.AttributeType == typeof(Column);
        //    var columns = typeof(T)
        //            .GetProperties()
        //            .Where(x => x.CustomAttributes.Any(columnOnly))
        //            .Select(p => new
        //            {
        //                Property = p,
        //                Column = p.GetCustomAttributes<Column>().First().ColumnIndex
        //            }).ToList();

        //    var rows = worksheet.Cell()   
        //        .Select(cell => cell.Start.Row)
        //        .Distinct()
        //        .OrderBy(x => x);            
            
        //    var collection = rows.Skip(2)
        //        .Select(row =>
        //        {
        //            var tnew = new T();
        //            columns.ForEach(col =>
        //            {                                            
        //            var val = worksheet.Cell(row,col);                    
        //            if (val.Value == null)
        //                {
        //                    col.Property.SetValue(tnew, null);
        //                    return;
        //                }
        //                if (col.Property.PropertyType == typeof(Int32))
        //                {
        //                    col.Property.SetValue(tnew, val.GetValue<int>());
        //                    return;
        //                }
        //                if (col.Property.PropertyType == typeof(double))
        //                {
        //                    col.Property.SetValue(tnew, val.GetValue<double>());
        //                    return;
        //                }
        //                if (col.Property.PropertyType == typeof(DateTime))
        //                {
        //                    col.Property.SetValue(tnew, val.GetValue<DateTime>());
        //                    return;
        //                }

        //                col.Property.SetValue(tnew, val.GetValue<string>());
        //            });

        //            return tnew;
        //        });
            
        //    return collection;
        //}

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static bool TryParseStringToInt(this string sVal)
        {
            if (string.IsNullOrEmpty(sVal)) return false;
            var tmpResult = int.TryParse(sVal, out _);
            if (tmpResult == false)
                return false;
            if (int.Parse(sVal) < 0)
                return false;

            return true;
        }

        public static bool TryParseStringToDecimal(this string sVal)
        {
            if (string.IsNullOrEmpty(sVal)) return false;
            var tmpResult = decimal.TryParse(sVal, out _);
            if (tmpResult == false)
                return false;
            if (decimal.Parse(sVal) < 0)
                return false;

            return true;
        }

        public static string  IsTrueFalseNullToString(this bool? x)
        {
            string y;
            if (x == null)
            {
                y = null;
            }
            else if (x == true)
            {
                y = "1";
            }
            else
            {
                y = "0";
            }
            return y;
        }      
    }
}
