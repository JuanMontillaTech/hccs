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
    public class SysUserCompanyController : ControllerBase
    {
        private readonly ISysRepository<SysUserCompany> _repSysUserCompany;
        private readonly ISysRepository<SysUser> _repSysUser;

        private readonly IMapper _mapper;
        private ICurrentUser _getCurrentUser;
        private int _dataSave;

        public SysUserCompanyController(ISysRepository<SysUserCompany> repSysUserCompany, IMapper mapper, ICurrentUser getCurrentUser, ISysRepository<SysUser> repSysUser)
        {
            _repSysUserCompany = repSysUserCompany;

            _mapper = mapper;
            this._getCurrentUser = getCurrentUser;
            _repSysUser = repSysUser;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] SysUserCompanyDto data)
        {
            var mapper = _mapper.Map<SysUserCompany>(data);

            var result = await _repSysUserCompany.InsertAsync(mapper);

            _dataSave = await _repSysUserCompany.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<SysUserCompanyDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<SysUserCompanyDto>(result);
            return Ok(Result<SysUserCompanyDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var dataSave = await _repSysUserCompany.Find(x => x.IsActive).AsQueryable() .ToListAsync();


            var mapperOut = _mapper.Map<SysUserCompanyDto[]>(dataSave);

            return Ok(Result<SysUserCompanyDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<SysUserCompanyDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var getSysUserCompany = _repSysUserCompany.Find(x => x.IsActive == true
                                           && (x.SysUser.Email.ToLower().Contains(filter.Search.Trim().ToLower()))  || 
                                           (x.SysCompany.CompanyName.ToLower().Contains(filter.Search.Trim().ToLower())))
                .Include(x=> x.SysCompany)
                .Include(x=> x.SysUser).ToList();

            int totalRecords = _repSysUserCompany.Find(t => t.IsActive).Count();
            var dataMaperOut = _mapper.Map<List<SysUserCompanyDto>>(getSysUserCompany);

            var listSysUserCompany = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<SysUserCompanyDto>>.Success(listSysUserCompany);
            return Ok(result);
        }
        [HttpGet("GetFilterUser")]
        [ProducesResponseType(typeof(Result<ICollection<SysUserCompanyDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilterUser([FromQuery] PaginationFilter filter)
        {
            var userEmail = _getCurrentUser.UserEmail();
            var user = _repSysUser.Find(x => x.IsActive && x.Email == userEmail).FirstOrDefault();
        
            var userId =user.Id;
            var getSysUserCompany = _repSysUserCompany.Find(x => x.IsActive == true && x.SysUserId == userId )
                .Include(x=> x.SysCompany)
               .ToList();

            int totalRecords = _repSysUserCompany.Find(t => t.IsActive).Count();
            var dataMaperOut = _mapper.Map<List<SysUserCompanyDto>>(getSysUserCompany);

            var listSysUserCompany = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<SysUserCompanyDto>>.Success(listSysUserCompany);
            return Ok(result);
        }
        

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repSysUserCompany.GetById(id);

            var mapperOut = _mapper.Map<SysUserCompanyDto>(dataSave);

            return Ok(Result<SysUserCompanyDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repSysUserCompany.GetById(id);

            data.IsActive = false;

            await _repSysUserCompany.Update(data);

            var save = await _repSysUserCompany.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<SysUserCompanyDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<SysUserCompanyDto>(data);

            return Ok(Result<SysUserCompanyDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SysUserCompanyDto updateDto)
        {
            var mapper = _mapper.Map<SysUserCompany>(updateDto);
            mapper.IsActive = true;
            var result = await _repSysUserCompany.Update(mapper);

            var dataSave = await _repSysUserCompany.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<SysUserCompanyDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<SysUserCompanyDto>(result);

            return Ok(Result<SysUserCompanyDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}