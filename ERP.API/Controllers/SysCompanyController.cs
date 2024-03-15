using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysCompanyController : ControllerBase
    {
        private readonly ISysRepository<SysCompany> _repSysCompany;

        private readonly IMapper _mapper;

        private int _dataSave;

        public SysCompanyController(ISysRepository<SysCompany> repSysCompany, IMapper mapper)
        {
            _repSysCompany = repSysCompany;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SysCompanyDto data)
        {
            var mapper = _mapper.Map<SysCompany>(data);

            var result = await _repSysCompany.InsertAsync(mapper);

            _dataSave = await _repSysCompany.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<SysCompanyDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<SysCompanyDto>(result);
            return Ok(Result<SysCompanyDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var dataSave = await _repSysCompany.Find(x => x.IsActive).AsQueryable() .ToListAsync();


            var mapperOut = _mapper.Map<SysCompanyDto[]>(dataSave);

            return Ok(Result<SysCompanyDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<SysCompanyDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var getSysCompany = _repSysCompany.Find(x => x.IsActive == true
                                           && (x.CompanyName.ToLower().Contains(filter.Search.Trim().ToLower())) ).ToList();

            int totalRecords = getSysCompany.Count();
            var dataMaperOut = _mapper.Map<List<SysCompanyDto>>(getSysCompany);

            var listSysCompany = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<SysCompanyDto>>.Success(listSysCompany);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repSysCompany.GetById(id);

            var mapperOut = _mapper.Map<SysCompanyDto>(dataSave);

            return Ok(Result<SysCompanyDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repSysCompany.GetById(id);

            data.IsActive = false;

            await _repSysCompany.Update(data);

            var save = await _repSysCompany.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<SysCompanyDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<SysCompanyDto>(data);

            return Ok(Result<SysCompanyDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SysCompanyDto updateDto)
        {
            var mapper = _mapper.Map<SysCompany>(updateDto);
            mapper.IsActive = true;
            var result = await _repSysCompany.Update(mapper);

            var dataSave = await _repSysCompany.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<SysCompanyDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<SysCompanyDto>(result);

            return Ok(Result<SysCompanyDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}