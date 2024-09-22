using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataWayController : ControllerBase
    {
        private readonly ISysRepository<DataWay> _repBox;

        private readonly IMapper _mapper;
        public DataWayController(ISysRepository<DataWay> _repBox, IMapper mapper)
        {

            this._repBox = _repBox;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] DataWayDto data)
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
