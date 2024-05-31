using System;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ERP.Domain.Entity;
using ERP.Infrastructure.Repositories;

namespace ERP.API.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
         

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
          
        }

        public Guid UserId()
        {

            return Guid.Empty;

        }

        public Guid Roll()
        {
            var stream = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                .Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "Roll").Value;
            return Guid.Parse(jti);
        }

        public string DataBaseName()
        {
            try
            {
                var stream = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                    .Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var jti = tokenS.Claims.First(claim => claim.Type == "DbName").Value;


                return jti;
            }
            catch (Exception )
            {
                return "";
            }
           
        }

        public string Sub()
        {
            var stream = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                .Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "Sub").Value;
            return jti;
        }

        public bool IsAnonymous()
        {
            return true;
        }

        public string UserEmail()
        {
            var stream = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                .Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var jti = tokenS.Claims.First(claim => claim.Type == "Email").Value;
            return tokenS.Claims.First(claim => claim.Type == "Email").Value;
        }

        public bool UserIsInRole()
        {
            return true;
        }
    }
}