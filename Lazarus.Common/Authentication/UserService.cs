using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentValidation.Validators;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;
using Lazarus.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace Lazarus.Common.Authentication
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            
        }

        public string GetAuthToken()
        {
            
            string t = _httpContext.HttpContext.Request.Headers["Authorization"];
            return t;
        }

        public CustomerCredential GetCustomerCredential(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = GetAuthToken();

            if (string.IsNullOrEmpty(token))
                return null;

            var user = TokenManager.GetPrincipal(token);
            if (user == null) return null;
 
            var result = new CustomerCredential();
            result.Email = TokenManager.GetClaim(ClaimStore.Email, token);
            result.Name = TokenManager.GetClaim(ClaimStore.Name, token);
            


            return result;
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext.HttpContext;
        }

        public ReceiverCredential GetReceiverCredential(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = GetAuthToken();

            if (string.IsNullOrEmpty(token))
                return null;

            var user = TokenManager.GetPrincipal(token);
            if (user == null) return null;

            var result = new ReceiverCredential();
            result.Email = TokenManager.GetClaim(ClaimStore.Email, token);
            result.Name = TokenManager.GetClaim(ClaimStore.Name, token);
            result.UserId = TokenManager.GetClaim(ClaimStore.UserId, token);
            result.Topic = TokenManager.GetClaim(ClaimStore.Topic, token);



            return result;
        }

        public UserCredential GetUserCredential(string token = "")
        {
            if (string.IsNullOrEmpty(token))
                token = GetAuthToken();

            if (string.IsNullOrEmpty(token))
                return null;

            var user = TokenManager.GetPrincipal(token);
            if (user == null) return null;
            var roles = ((ClaimsIdentity)user.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();
            var result = new UserCredential();
            result.Email = TokenManager.GetClaim(ClaimStore.Email, token);
            result.UserId = TokenManager.GetClaim(ClaimStore.UserId, token);
            var roleString= TokenManager.GetClaim(ClaimStore.Role, token);
       
            result.Roles = roleString.ToObject<List<string>>();

            return result;
        }
    }
}
