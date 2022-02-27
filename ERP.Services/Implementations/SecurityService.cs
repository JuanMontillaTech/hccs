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
        public readonly IGenericRepository<Sys_User> RepositoryUser;
        private const string SECRET_KEY = "aBCDE4JNKNLKDNARVAJN545N4J5N4PL4H4P44H5JBSSDBNF3453S2223KJNH";
        public static readonly SymmetricSecurityKey SINGING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public SecurityService(IGenericRepository<Sys_User> repositoryUser)
        {
            RepositoryUser = repositoryUser;
        }

        public async Task<Result<string>> LoginAsync(string Email, string password)
        {
            var result = await RepositoryUser.GetAll();

            var User = result.Where(x => x.Email == Email && x.Password == password).FirstOrDefault();

            if (User != null)
            {


                var credentials = new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256);

                var header = new JwtHeader(credentials);

                DateTime Expiry = DateTime.UtcNow.AddDays(1);

                int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

                var payload = new JwtPayload
            {
                {"Sub",  User.Email },
                {"Name", User.Email},
                {"Email",User.Email },
                {"Roll",User.Email },
                {"RollName",User.Email },
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
