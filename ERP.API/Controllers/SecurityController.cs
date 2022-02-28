using ERP.Domain.Dtos;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        public readonly ISecurityService SecurityService;

        public SecurityController(ISecurityService securityService)
        {
            SecurityService = securityService;
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        [HttpPost("Login")]
     
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto data)
        {
            var result = await SecurityService.LoginAsync(data.Email, data.Password);

            return Ok(result);
        }

    }
}
