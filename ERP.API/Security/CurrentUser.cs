using ERP.Domain;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
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
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }

        public bool UserIsInRole()
        {
            return true;
        }
        

    }
}
