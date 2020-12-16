using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Utilities
{
    public static class AgingUtilities
    {

        public static DateTime StampStartAge(DateTime date, List<DateTime> holidays, bool beforeisHoliday = false)
        {

            var _isHoliday = date.ToString("ddd") == "Sat" || date.ToString("ddd") == "Sun" ||
                             holidays.Any(s => s.Date == date.Date);
            if (_isHoliday)
            {

                return StampStartAge(date.Date.Add(new TimeSpan(8, 0, 0)).AddDays(1), holidays, true);

            }
            else if (beforeisHoliday == false && date > date.Date.Add(new TimeSpan(12, 0, 0)) && date < date.Date.Add(new TimeSpan(13, 0, 0)))
            {
                return date.Date.Add(new TimeSpan(13, 0, 0));
            }
            else if (beforeisHoliday == false && date > date.Date.Add(new TimeSpan(17, 0, 0)))
            {
                return StampStartAge(date.Date.AddDays(1), holidays);
            }
            else if (beforeisHoliday == false && date < date.Date.Add(new TimeSpan(8, 0, 0)))
            {
                return date.Date.Add(new TimeSpan(8, 0, 0));

            }
            else
            {
                return date;
            }

        }
        private static string ConvertAgingToDisplay(double age)
        {
            if (age == 0 || age < 60) return "";
            var displayDay = Math.Floor(age / 480);
            var minutes = age - (displayDay * 480);
            var displayHours = Math.Floor(minutes / 60);
            return string.Format("{0}d:{1}h", displayDay, displayHours);
        }


        public static double Cal(DateTime? startAgeDate, List<DateTime> holidays, DateTime datenow)
        {

            if (startAgeDate.HasValue == false) return 0;


            var datediff = (datenow - startAgeDate.Value.Date).TotalDays;
            double total = 0;

            for (int i = 0; i <= datediff; i++)
            {
                if (i != 0)
                    startAgeDate = startAgeDate.Value.AddDays(1);
                // currentday
                var isHoliday = false;
                var d = startAgeDate.Value.ToString("ddd");
                if (startAgeDate.Value.ToString("ddd") == "Sat" || startAgeDate.Value.ToString("ddd") == "Sun" ||
                    holidays.Any(s => s.Date == startAgeDate.Value.Date))
                    isHoliday = true;

                if (startAgeDate.Value.Date == datenow.Date && isHoliday == false)
                {
                    var CalDate = datenow;
                    TimeSpan time = new TimeSpan(0, 17, 0, 0);
                    var FinishWorkingTime = datenow.Date.Add(time);
                    if (datenow > FinishWorkingTime)
                        CalDate = FinishWorkingTime;
                    if (i > 0)
                        startAgeDate = startAgeDate.Value.Date.Add(new TimeSpan(8, 0, 0));
                    if (startAgeDate < CalDate)
                        total += (CalDate - startAgeDate.Value).TotalMinutes;

                    var breaktime = datenow.Date.Add(new TimeSpan(13, 0, 0));
                    if (startAgeDate.Value < breaktime && datenow > breaktime)
                        total = total - 60;
                }
                else if (isHoliday == false && i != 0)
                {
                    total = total + 480;
                }
                else if (isHoliday == false && i == 0)
                {

                    TimeSpan time = new TimeSpan(0, 17, 0, 0);

                    var aa = startAgeDate.Value.Date.Add(time);
                    total += (startAgeDate.Value.Date.Add(time) - startAgeDate).Value.TotalMinutes;

                    var breaktime = startAgeDate.Value.Date.Add(new TimeSpan(13, 0, 0));
                    if (startAgeDate < breaktime)
                        total = total - 60;
                }
            }
            return total;
        }
        public static string CalAging(DateTime? startAgeDate, List<DateTime> holidays, DateTime datenow)
        {
            var aging = Cal(startAgeDate, holidays, datenow);

            return ConvertAgingToDisplay(aging);
        }
    }
}
