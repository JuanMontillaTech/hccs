using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService SecurityService;
        private readonly ISysRepository<DataWay> _repBox;
        private readonly IMapper _mapper;
        private readonly IDirectSql RepDynamic;
        public IConfiguration _config { get; }
        public SecurityController(IDirectSql repDynamic, ISecurityService securityService, ISysRepository<DataWay> repBox, IMapper _mapper, IConfiguration config)
        {
            RepDynamic = repDynamic;
            SecurityService = securityService;
            this._repBox = repBox;
            this._mapper = _mapper;
            _config = config;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto data)
        {
            try
            {
                var result = await SecurityService.LoginAsync(data.Email, data.Password);
                await SaveAuditLog(data.Email, result.Data != null, result.Data == null ? "Usuario no ingreso" : null);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await SaveAuditLog(data.Email, false, ex.Message);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Save Log Information
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="IsSuccess"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        private async Task SaveAuditLog(string Email, bool IsSuccess, string Error)
        {
            var auditLog = new AuditLog
            {
                UserId = Email,
                LoginTimestamp = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString(),
                IsSuccess = IsSuccess,
                Error = Error
            };
            string connectionString = _config.GetConnectionString("SysDefaultConnection");

            string sqlInsert = @"
    INSERT INTO AuditLogs (UserId, LoginTimestamp, IpAddress, UserAgent, IsSuccess, Error) 
    VALUES (@UserId, @LoginTimestamp, @IpAddress, @UserAgent, @IsSuccess, @Error)";

            var parameters = new List<ReportParametersDto>
                        {
                        new ReportParametersDto { paramName = "UserId", paramValue = auditLog.UserId },
                        new ReportParametersDto { paramName = "LoginTimestamp", paramValue = auditLog.LoginTimestamp.ToString() },
                        new ReportParametersDto { paramName = "IpAddress", paramValue = auditLog.IpAddress },
                        new ReportParametersDto { paramName = "UserAgent", paramValue = auditLog.UserAgent },
                        new ReportParametersDto { paramName = "IsSuccess", paramValue = auditLog.IsSuccess.ToString() },
                        new ReportParametersDto { paramName = "Error", paramValue = auditLog.Error }
                    };
            await RepDynamic.QueryDynamic(sqlInsert, parameters, connectionString);
        }

        [HttpGet("GetTokenWith")]
        public async Task<IActionResult> GetTokenWith([FromQuery] Guid Companyid)
        {
            var result = await SecurityService.SetDb(Companyid);

            return Ok(result);
        }




        [HttpGet("GetTokenFinal")]
        public async Task<IActionResult> GetTokenFinal([FromQuery] Guid RolId)
        {
            var result = await SecurityService.GetToken(RolId);

            return Ok(result);
        }

        [HttpPost("TranferenciaImport")]
        public async Task<IActionResult> Tranferencia([FromBody] DataWayDto data)
        {
            var mapper = _mapper.Map<DataWay>(data);
            mapper.Code = GenerarCodigoAleatorio();
            var result = await _repBox.InsertAsync(mapper);

            var _dataSave = await _repBox.SaveChangesAsync();

            if (_dataSave != 1)
                return Ok(Result<DataWayDto>.Fail(MessageCodes.ErrorCreating, "API"));

            var mapperOut = _mapper.Map<DataWayDto>(result);

            return Ok(Result<DataWayDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        public static string GenerarCodigoAleatorio()
        {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var codigo = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                int indiceAleatorio = random.Next(caracteresPermitidos.Length);
                codigo.Append(caracteresPermitidos[indiceAleatorio]);
            }

            return codigo.ToString();
        }

    }

}
