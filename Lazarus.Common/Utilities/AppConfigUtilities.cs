using Autofac;
using Lazarus.Common.Authentication;
using Lazarus.Common.DI;
using Lazarus.Common.Domain.Seedwork;
using Lazarus.Common.Interface;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lazarus.Common.Utilities
{
    public class AppConfigUtilities
    {
        public static IConfiguration _configuration { get; set; }
        public static CultureInfo CreateCulture(string langCode)
        {
            switch (langCode)
            {
                case "th":
                {
                    return CultureInfo.CreateSpecificCulture("th-TH");
                }

                default:
                {
                    return CultureInfo.CreateSpecificCulture("en-GB");
                }
            }
        }
        public static string GetDomain()
        {
            var user = DomainEvents._Container.Resolve<IUserService>();
            var context = user.GetHttpContext();
            return context.Request.Host.Host;


        }

        public static T GetAppConfig<T>(string key)
        {

         
            var value = AppConfigUtilities._configuration.GetSection("AppSettings:"+key).Value; 
            if (string.IsNullOrEmpty(value)) throw new Exception(string.Format("Config {0} value not found", key));
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                var result = (T)converter.ConvertFromString(null,
                    CultureInfo.InvariantCulture, value);
                return result;

            }
            catch (Exception)
            {
                throw new Exception(string.Format("Invalid Convert type Value:{0}", key));
            }


        }
    }
}
