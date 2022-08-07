


using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
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
    public class ConfigurationPurchaseController : ControllerBase
    {
        public readonly IGenericRepository<ConfigurationPurchase> RepConfigurationSells;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConfigurationPurchaseController(IGenericRepository<ConfigurationPurchase> repConfigurationSells, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepConfigurationSells = repConfigurationSells;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        [HttpGet("GetFirst")]
        public async Task<IActionResult> GetFirst()
        {
            var DataSave = await RepConfigurationSells.GetAll();
            var row = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<ConfigurationPurchaseDto>(row);

            return Ok(Result<ConfigurationPurchaseDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConfigurationSells.GetById(id);

            var mapperOut = _mapper.Map<ConfigurationPurchaseDto>(DataSave);

            return Ok(Result<ConfigurationPurchaseDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConfigurationPurchaseDto _UpdateDto)
        {

            var mapper = _mapper.Map<ConfigurationPurchase>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConfigurationSells.Update(mapper);

            var DataSave = await RepConfigurationSells.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConfigurationPurchaseDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConfigurationPurchaseDto>(result);

            return Ok(Result<ConfigurationPurchaseDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
