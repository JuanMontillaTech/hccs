using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Model.Dtos;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class ManufactureStatusController : ControllerBase
    {
        public readonly IGenericRepository<ManufactureStatus> RepManufactureStatuss;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ManufactureStatusController(IGenericRepository<ManufactureStatus> repManufactureStatuss, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepManufactureStatuss = repManufactureStatuss;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ManufactureStatusDto data)
        {
            var mapper = _mapper.Map<ManufactureStatus>(data);

            var result = await RepManufactureStatuss.Insert(mapper);

            var DataSave = await RepManufactureStatuss.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ManufactureStatusDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ManufactureStatusDto>(result);

            return Ok(Result<ManufactureStatusDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            var Filter = await RepManufactureStatuss.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<ManufactureStatusDto[]>(Filter);

            return Ok(Result<ManufactureStatusDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ManufactureStatusDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepManufactureStatuss.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepManufactureStatuss.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<ManufactureStatusDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ManufactureStatusDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepManufactureStatuss.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<ManufactureStatusDto>(Filter);

            return Ok(Result<ManufactureStatusDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepManufactureStatuss.GetById(id);

            var mapperOut = _mapper.Map<ManufactureStatusDto>(DataSave);

            return Ok(Result<ManufactureStatusDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepManufactureStatuss.GetById(id);

            Data.IsActive = false;

            await RepManufactureStatuss.Update(Data);

            var save = await RepManufactureStatuss.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ManufactureStatusDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ManufactureStatusDto>(Data);

            return Ok(Result<ManufactureStatusDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ManufactureStatusDto _UpdateDto)
        {

            var mapper = _mapper.Map<ManufactureStatus>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepManufactureStatuss.Update(mapper);

            var DataSave = await RepManufactureStatuss.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ManufactureStatusDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ManufactureStatusDto>(result);

            return Ok(Result<ManufactureStatusDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
