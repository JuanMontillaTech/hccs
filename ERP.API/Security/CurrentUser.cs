using ERP.Domain;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP.API.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string DataBaseName()
        {
           return "";
        }

        public bool IsAnonymous()
        {
            return true;
        }

        public string UserEmail()
        {
            var stream = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
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
