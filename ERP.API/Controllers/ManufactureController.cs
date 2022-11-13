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
    public class ManufactureController : ControllerBase
    {
        public readonly IGenericRepository<Manufacture> RepManufactures;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ManufactureController(IGenericRepository<Manufacture> repManufactures, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepManufactures = repManufactures;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ManufactureDto data)
        {
            var mapper = _mapper.Map<Manufacture>(data);

            var result = await RepManufactures.Insert(mapper);

            var DataSave = await RepManufactures.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ManufactureDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ManufactureDto>(result);

            return Ok(Result<ManufactureDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            var Filter = await RepManufactures.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<ManufactureDto[]>(Filter);

            return Ok(Result<ManufactureDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ManufactureDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepManufactures.Find(x => x.IsActive == true
            && (x.ManufactureStatus.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepManufactures.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<ManufactureDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ManufactureDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepManufactures.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<ManufactureDto>(Filter);

            return Ok(Result<ManufactureDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepManufactures.GetById(id);

            var mapperOut = _mapper.Map<ManufactureDto>(DataSave);

            return Ok(Result<ManufactureDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepManufactures.GetById(id);

            Data.IsActive = false;

            await RepManufactures.Update(Data);

            var save = await RepManufactures.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ManufactureDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ManufactureDto>(Data);

            return Ok(Result<ManufactureDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ManufactureDto _UpdateDto)
        {

            var mapper = _mapper.Map<Manufacture>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepManufactures.Update(mapper);

            var DataSave = await RepManufactures.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ManufactureDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ManufactureDto>(result);

            return Ok(Result<ManufactureDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
