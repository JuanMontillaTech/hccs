
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
    public class ConfigurationSellController : ControllerBase
    {
        public readonly IGenericRepository<ConfigurationSell> RepConfigurationSells;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConfigurationSellController(IGenericRepository<ConfigurationSell> repConfigurationSells, IMapper mapper, IHttpContextAccessor httpContextAccessor)
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

            var mapperOut = _mapper.Map<ConfigurationSellDto>(row);

            return Ok(Result<ConfigurationSellDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        
        
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConfigurationSells.GetById(id);

            var mapperOut = _mapper.Map<ConfigurationSellDto>(DataSave);

            return Ok(Result<ConfigurationSellDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        
 
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConfigurationSellDto _UpdateDto)
        {
            //var data = await RepConfigurationSells.GetAll();
            //var firstRecord = data.FirstOrDefault();
            //_UpdateDto.Id = firstRecord.Id;

            var mapper = _mapper.Map<ConfigurationSell>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConfigurationSells.Update(mapper);
           
            var DataSave = await RepConfigurationSells.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConfigurationSellDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConfigurationSellDto>(result);

            return Ok(Result<ConfigurationSellDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
