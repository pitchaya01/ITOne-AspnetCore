using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using Lazarus.Common.Authentication;
using Lazarus.Common.DI;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;


namespace Lazarus.Common.Utilities
{
    
    public static class UserUtilities
    {
  
        public static ReceiverCredential GetReceiver()
        {
            if (DomainEvents._Container == null) return null;
            var userService = DomainEvents._Container.Resolve<IUserService>();
            var u = userService.GetReceiverCredential();
            return u;
        }
        public static UserCredential GetUser()
        {
            if (DomainEvents._Container == null) return null;
            var userService =DomainEvents._Container.Resolve<IUserService>();
            var u = userService.GetUserCredential();
   
            return u;
        }

        public static string GetToken()
        {
            var userService = DomainEvents._Container.Resolve<IUserService>();
            var token = userService.GetAuthToken();
            return token;
        }
    }
}
