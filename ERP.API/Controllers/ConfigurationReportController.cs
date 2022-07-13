
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class ConfigurationReportController : ControllerBase
    {
        public readonly IGenericRepository<ConfigurationReport> RepConfigurationReports;

        private readonly IMapper _mapper;
        public ConfigurationReportController(IGenericRepository<ConfigurationReport> repConfigurationReports, IMapper mapper)
        {
            RepConfigurationReports = repConfigurationReports;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ConfigurationReportDto data)
        {
            var mapper = _mapper.Map<ConfigurationReport>(data);

            var result = await RepConfigurationReports.Insert(mapper);

            var DataSave = await RepConfigurationReports.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConfigurationReportIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ConfigurationReportIdDto>(result);

            return Ok(Result<ConfigurationReportIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepConfigurationReports.GetAll();
            var dataActive = DataSave.Where(x => x.IsActive).ToList();
            var mapperOut = _mapper.Map<List<ConfigurationReportIdDto>>(dataActive);

            return Ok(Result<List<ConfigurationReportIdDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAccountByCode")]
        public async Task<IActionResult> GetAccountByCode([FromQuery] string Code)
        {
            var query = await RepConfigurationReports.Find(x => x.Code == Code)
             .Include(x => x.LedgerAccount).OrderBy(x => x.Index).ToListAsync();
            return Ok(Result<List<ConfigurationReport>>.Success(query, MessageCodes.AllSuccessfully()));
     

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConfigurationReports.GetById(id);

            var mapperOut = _mapper.Map<ConfigurationReportIdDto>(DataSave);

            return Ok(Result<ConfigurationReportIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepConfigurationReports.GetById(id);

            Data.IsActive = false;

            await RepConfigurationReports.Update(Data);

            var save = await RepConfigurationReports.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ConfigurationReportIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ConfigurationReportIdDto>(Data);

            return Ok(Result<ConfigurationReportIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConfigurationReportIdDto _UpdateDto)
        {

            var mapper = _mapper.Map<ConfigurationReport>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConfigurationReports.Update(mapper);

            var DataSave = await RepConfigurationReports.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConfigurationReportIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConfigurationReportIdDto>(result);

            return Ok(Result<ConfigurationReportIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
