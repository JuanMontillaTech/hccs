using ERP.Domain.Command; 
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using System; 
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks; 
using ERP.Domain.Constants;

namespace ERP.Services.Implementations
{
    public class SecurityService : ISecurityService
    {
      
        private const string SECRET_KEY = "aBCDE4JNKNLKDNARVAJN545N4J5N4PL4H4P44H5JBSSDBNF3453S2223KJNH";
        private static readonly SymmetricSecurityKey SINGING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        private readonly ISysRepository<SysUser> _repSysUser; 
        private readonly ISysRepository<SysCompany> _repSysCompany;
        private readonly IGenericRepository<Roll> _repRoll;
        private readonly ICurrentUser  _currentUser;

        public SecurityService(ISysRepository<SysUser> repSysUser,   ISysRepository<SysCompany> repSysCompany, ICurrentUser currentUser, IGenericRepository<Roll> repRoll)
        {
            _repSysUser = repSysUser;
         
            _repSysCompany = repSysCompany;
            _currentUser = currentUser;
            _repRoll = repRoll;
        }


        public async Task<Result<string>> LoginAsync(string Email, string password)
        {
            
            var dbUser = await _repSysUser.Find(x => x.IsActive && x.Email == Email && x.Password == password)
                .FirstOrDefaultAsync();
      
            if (dbUser != null)
            {
                var tokenString = GetTokenString("","","",Email,"","");

                return Result<string>.Success(tokenString);
            }
            else
            {
                return Result<string>.Fail(MessageCodes.SecurityException, "400", "", "");
            }

        }

        public  async Task<Result<string>> SetDb( Guid companyId)
        {

            var roll = await _repSysCompany.GetById(companyId);

             var tokenString =GetTokenString (_currentUser.Sub(), _currentUser.UserEmail()
                 , roll.DataBaseName, _currentUser.UserEmail(),
                 "", "");
            
            return Result<string>.Success(tokenString);
             
        }

        public Task<Result<string>> GetToken(Guid UserRollId)
        {
            
            var tokenString =GetTokenString (_currentUser.UserEmail(), "", _currentUser.DataBaseName(), _currentUser.UserEmail(), UserRollId.ToString(), "");
       
            return Task.FromResult(Result<string>.Success(tokenString));
        }

        private static string GetTokenString(string Sub,string Name,string DbName,string Email,string Roll,string RollName)
        {
            var credentials = new SigningCredentials(SINGING_KEY, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            DateTime Expiry = DateTime.UtcNow.AddDays(1);

            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new JwtPayload
            {
                { "Sub", Sub },
                { "Name", Name },
                { "DbName", DbName },
                { "Email", Email },
                { "Roll", Roll },
                { "RollName", RollName },
                { "exp", ts },
                { "iss", "https://localhost:44319" }, //Issuer - la parte que generera el JWT
                { "aud", "https://localhost:44319" } //Audicen - la direcion del recuerso
            };

            var secToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken);
            return tokenString;
        }
    }
}
