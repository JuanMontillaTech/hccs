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
    public class TransactionLocationTransactionController : ControllerBase
    {
        public readonly IGenericRepository<TransactionLocationTransaction> RepTransactionLocationTransactions;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionLocationTransactionController(IGenericRepository<TransactionLocationTransaction> repTransactionLocationTransactions, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepTransactionLocationTransactions = repTransactionLocationTransactions;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionLocationTransactionDto data)
        {
            var mapper = _mapper.Map<TransactionLocationTransaction>(data);

            var result = await RepTransactionLocationTransactions.Insert(mapper);

            var DataSave = await RepTransactionLocationTransactions.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(result);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            var Filter = await RepTransactionLocationTransactions.Find(x => x.IsActive == true).ToListAsync();

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto[]>(Filter);

            return Ok(Result<TransactionLocationTransactionDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionLocationTransactionDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepTransactionLocationTransactions.Find(x => x.IsActive == true
            && (x.TransactionLocation.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            || (x.Transaction.Code.ToLower().Contains(filter.Search.Trim().ToLower()))
            || (x.Transaction.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepTransactionLocationTransactions.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<TransactionLocationTransactionDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionLocationTransactionDto>>.Success(List);
            return Ok(Result);

        }



        [HttpGet("GetDefault")]
        public async Task<IActionResult> GetDefault()
        {
            var DataSave = await RepTransactionLocationTransactions.GetAll();

            var Filter = DataSave.FirstOrDefault();

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(Filter);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTransactionLocationTransactions.GetById(id);

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(DataSave);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTransactionLocationTransactions.GetById(id);

            Data.IsActive = false;

            await RepTransactionLocationTransactions.Update(Data);

            var save = await RepTransactionLocationTransactions.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(Data);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionLocationTransactionDto _UpdateDto)
        {

            var mapper = _mapper.Map<TransactionLocationTransaction>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTransactionLocationTransactions.Update(mapper);

            var DataSave = await RepTransactionLocationTransactions.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(result);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
