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
    public class TransactionLocationController : ControllerBase
    {
        public readonly IGenericRepository<TransactionLocation> RepTransactionLocations;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionLocationController(IGenericRepository<TransactionLocation> repTransactionLocations, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepTransactionLocations = repTransactionLocations;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionLocationDto data)
        {
            var mapper = _mapper.Map<TransactionLocation>(data);

            var result = await RepTransactionLocations.Insert(mapper);

            var DataSave = await RepTransactionLocations.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TransactionLocationDto>(result);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            var Filter = await RepTransactionLocations.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<TransactionLocationDto[]>(Filter);

            return Ok(Result<TransactionLocationDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionLocationDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepTransactionLocations.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepTransactionLocations.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<TransactionLocationDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionLocationDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepTransactionLocations.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<TransactionLocationDto>(Filter);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTransactionLocations.GetById(id);

            var mapperOut = _mapper.Map<TransactionLocationDto>(DataSave);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTransactionLocations.GetById(id);

            Data.IsActive = false;

            await RepTransactionLocations.Update(Data);

            var save = await RepTransactionLocations.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionLocationDto>(Data);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionLocationDto _UpdateDto)
        {

            var mapper = _mapper.Map<TransactionLocation>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTransactionLocations.Update(mapper);

            var DataSave = await RepTransactionLocations.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionLocationDto>(result);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
