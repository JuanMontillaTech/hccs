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
    public class TaxesController : ControllerBase
    {
        public readonly IGenericRepository<Taxes> RepTaxess;

        private readonly IMapper _mapper;
        public TaxesController(IGenericRepository<Taxes> repTaxess, IMapper mapper)
        {
            RepTaxess = repTaxess;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TaxesDto data)
        {
            var mapper = _mapper.Map<Taxes>(data);

            var result = await RepTaxess.InsertAsync(mapper);

            var DataSave = await RepTaxess.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TaxesDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TaxesDto>(result);

            return Ok(Result<TaxesDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepTaxess.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<List<TaxesDto>>(Filter);

            return Ok(Result<List<TaxesDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TaxesDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepTaxess.Find(x => x.IsActive == true
            && (x.Code.ToLower().Contains(filter.Search.Trim().ToLower()))  ).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<TaxesDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TaxesDto>>.Success(List);
            return Ok(Result);

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTaxess.GetById(id);

            var mapperOut = _mapper.Map<TaxesDto>(DataSave);

            return Ok(Result<TaxesDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTaxess.GetById(id);

            Data.IsActive = false;

            await RepTaxess.Update(Data);

            var save = await RepTaxess.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TaxesDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TaxesDto>(Data);

            return Ok(Result<TaxesDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TaxesDto _UpdateDto)
        {

            var mapper = _mapper.Map<Taxes>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTaxess.Update(mapper);

            var DataSave = await RepTaxess.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TaxesDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TaxesDto>(result);

            return Ok(Result<TaxesDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
