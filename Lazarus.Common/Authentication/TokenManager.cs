using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lazarus.Common.Model;
using Lazarus.Common.Utilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Lazarus.Common.Authentication
{
    public enum ClaimStore
    {
        Name,
        Email,
        UserId,
        HotelId,
        Role,
        Topic
    
    }
    public class TokenManager
    {
        private static string Secret = AppConfigUtilities.GetAppConfig<string>("JwtKey");

        public static string GenerateToken(CustomerCredential user, DateTime expireDate)
        {


            var claims = new List<Claim>
                    {
                                       new Claim(ClaimTypes.Name, user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Email.ToString()),
                            new Claim(ClaimStore.Email.ToString(), user.Email),
                            new Claim(ClaimStore.Name.ToString(), user.Name),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigUtilities.GetAppConfig<string>("JwtKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble("8"));

            var token = new JwtSecurityToken(
                issuer: AppConfigUtilities.GetAppConfig<string>("JwtIssuer"),
                audience: AppConfigUtilities.GetAppConfig<string>("JwtAudience"),
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            string t = new JwtSecurityTokenHandler().WriteToken(token);


            return t;

        }
        public static string GenerateToken(ReceiverCredential user, DateTime expireDate)
        {


            var claims = new List<Claim>
                    {
                                       new Claim(ClaimTypes.Name, user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Email.ToString()),
                            new Claim(ClaimStore.Email.ToString(), user.Email),
                            new Claim(ClaimStore.Topic.ToString(), user.Topic)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigUtilities.GetAppConfig<string>("JwtKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble("8"));

            var token = new JwtSecurityToken(
                issuer: AppConfigUtilities.GetAppConfig<string>("JwtIssuer"),
                audience: AppConfigUtilities.GetAppConfig<string>("JwtAudience"),
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            string t = new JwtSecurityTokenHandler().WriteToken(token);


            return t;

        }
        public static string GenerateToken(UserCredential user,DateTime expireDate)
        {


            var claims = new List<Claim>
                    {
                                       new Claim(ClaimTypes.Name, user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Email),
                            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.UserId.ToString()),
                            new Claim(ClaimStore.Email.ToString(), user.Email),
                            new Claim(ClaimStore.UserId.ToString(), user.UserId),
                            new Claim(ClaimStore.Role.ToString(), user.Roles.ToJSON()),
        };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigUtilities.GetAppConfig<string>("JwtKey")));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var expires = DateTime.Now.AddDays(Convert.ToDouble("8"));

                            var token = new JwtSecurityToken(
                                issuer: AppConfigUtilities.GetAppConfig<string>("JwtIssuer"),
                                audience: AppConfigUtilities.GetAppConfig<string>("JwtAudience"),
                                claims: claims,
                                expires: expires,
                                signingCredentials: creds
                            );

                            string t= new JwtSecurityTokenHandler().WriteToken(token);


            return t;

        }

        public static string GetClaim(ClaimStore key, string token)
        {
            var val = GetPrincipal(token).Claims.Where(s => s.Type == key.ToString()).FirstOrDefault();
            if (val == null) return "";


            return val.Value;
        }

        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return username;
        }


        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                token= token.StartsWith("Bearer ") ? token.Substring(7) : token;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                  
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
  
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AppConfigUtilities.GetAppConfig<string>("JwtIssuer"),
                    ValidAudience = AppConfigUtilities.GetAppConfig<string>("JwtAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))

                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
