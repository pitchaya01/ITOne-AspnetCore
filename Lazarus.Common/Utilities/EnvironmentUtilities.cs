using Lazarus.Common.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using System.Web;

namespace Lazarus.Common.Utilities
{
    public class EnvironmentUtilities
    {
 
        private static string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }
    }
}