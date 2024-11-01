﻿using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService SecurityService;
        private readonly ISysRepository<DataWay> _repBox;
        private readonly IMapper _mapper;
        public SecurityController(ISecurityService securityService, ISysRepository<DataWay> repBox, IMapper _mapper)
        {
            SecurityService = securityService;
            this._repBox = repBox;
            this._mapper = _mapper;
        }
   

        [HttpPost("Login")] 
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto data)
        {
            var result = await SecurityService.LoginAsync(data.Email, data.Password);

            return Ok(result);
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
