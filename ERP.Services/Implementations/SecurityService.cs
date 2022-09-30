using ERP.Domain.Command;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ERP.Domain.Constants;

namespace ERP.Services.Implementations
{
    public class SecurityService : ISecurityService
    {
      
        private const string SECRET_KEY = "aBCDE4JNKNLKDNARVAJN545N4J5N4PL4H4P44H5JBSSDBNF3453S2223KJNH";
        public static readonly SymmetricSecurityKey SINGING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

  

        public async Task<Result<string>> LoginAsync(string Email, string password)
        {
            var DbUser = await new DapperServer<Sys_User>().Select($"  Email = '{ Email}' and  Password= '{password}'");
      
            if (DbUser != null)
            {
                var user = DbUser.FirstOrDefault();
                if (user == null)
                {
                    return Result<string>.Fail(MessageCodes.SecurityException, "400", "", "");
                }
                var credentials = new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(credentials);

                DateTime Expiry = DateTime.UtcNow.AddDays(1);

                int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

                var payload = new JwtPayload
            {
                {"Sub",  Email },
                {"Name", Email },
                {"DbName", user.DataBaseName },
                {"Email",Email },
                {"Roll",Email },
                {"RollName",Email },
                {"exp",ts },
                {"iss", "https://localhost:44319"}, //Issuer - la parte que generera el JWT
                {"aud", "https://localhost:44319" } //Audicen - la direcion del recuerso
            };

                var secToken = new JwtSecurityToken(header, payload);

                var handler = new JwtSecurityTokenHandler();

                var tokenString = handler.WriteToken(secToken); // SegurityToken   

                return Result<string>.Success(tokenString);
            }
            else
            {
                return Result<string>.Fail(MessageCodes.SecurityException, "400", "", "");
            }

        }
    }
}
