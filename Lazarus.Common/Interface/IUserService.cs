using Lazarus.Common.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Interface
{
    public interface IUserService
    {
        UserCredential GetUserCredential(string tokne="");
        CustomerCredential GetCustomerCredential(string tokne = "");
        ReceiverCredential GetReceiverCredential(string tokne = "");
        string GetAuthToken();
         HttpContext GetHttpContext();


    }
}
