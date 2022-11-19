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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class NumerationController : ControllerBase
    {
        public readonly IGenericRepository<Numeration> RepNumerations;

        private readonly IMapper _mapper;
        public NumerationController(IGenericRepository<Numeration> repNumerations, IMapper mapper)
        {
            RepNumerations = repNumerations;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] NumerationDto data)
        {
            var mapper = _mapper.Map<Numeration>(data);

            var result = await RepNumerations.InsertAsync(mapper);

            var DataSave = await RepNumerations.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<NumerationDto>(result);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepNumerations.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<List<NumerationDto>>(Filter);

            return Ok(Result<List<NumerationDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<NumerationDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepNumerations.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Prefix.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepNumerations.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<NumerationDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<NumerationDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepNumerations.GetById(id);

            var mapperOut = _mapper.Map<NumerationDto>(DataSave);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepNumerations.GetById(id);

            Data.IsActive = false;

            await RepNumerations.Update(Data);

            var save = await RepNumerations.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<NumerationDto>(Data);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] NumerationDto _UpdateDto)
        {

            var mapper = _mapper.Map<Numeration>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepNumerations.Update(mapper);

            var DataSave = await RepNumerations.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<NumerationDto>(result);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
