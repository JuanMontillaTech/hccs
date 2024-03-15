using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.API.ValidatorDto;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionLocationTransactionController : ControllerBase
    {
        private readonly IGenericRepository<TransactionLocationTransaction> _repTransactionLocationTransaction;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public TransactionLocationTransactionController(IGenericRepository<TransactionLocationTransaction> repTransactionLocationTransaction, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repTransactionLocationTransaction = repTransactionLocationTransaction;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionLocationTransactionDto data)
        {

            var mapper = _mapper.Map<TransactionLocationTransaction>(data);

            var result = await _repTransactionLocationTransaction.InsertAsync(mapper);
            try
            {
                _dataSave = await _repTransactionLocationTransaction.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(result);
                return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<TransactionLocationTransactionDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repTransactionLocationTransaction.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.TransactionLocation).Include(x => x.Transactions).ThenInclude(x=> x.Contact).OrderBy(x => x.TransactionLocation.Name).ToListAsync();

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto[]>(dataSave);

            return Ok(Result<TransactionLocationTransactionDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionLocationTransactionDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repTransactionLocationTransaction.Find(x => x.IsActive == true
            && (x.Transactions.Code.ToLower().Contains(filter.Search.Trim().ToLower()))
            && (x.TransactionLocation.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Transactions.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<TransactionLocationTransactionDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionLocationTransactionDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repTransactionLocationTransaction.GetById(id);

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(dataSave);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetByTransactionLocationId")]
        public async Task<IActionResult> GetByTransactionLocationId([FromQuery] Guid id)
        {
            var dataSave = await _repTransactionLocationTransaction.Find(x => x.IsActive && x.TransactionLocationId == id).AsQueryable()
              .Include(x => x.TransactionLocation).Include(x => x.Transactions).ThenInclude(x => x.TransactionsDetails).ThenInclude(x=> x.Concept).OrderBy(x => x.TransactionLocation.Name).ToListAsync();


            var mapperOut = _mapper.Map<TransactionLocationTransactionDto[]>(dataSave);

            return Ok(Result<TransactionLocationTransactionDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repTransactionLocationTransaction.GetById(id);

            data.IsActive = false;

            await _repTransactionLocationTransaction.Update(data);

            var save = await _repTransactionLocationTransaction.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(data);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionLocationTransactionDto updateDto)
        {
            var mapper = _mapper.Map<TransactionLocationTransaction>(updateDto);
            mapper.IsActive = true;
            var result = await _repTransactionLocationTransaction.Update(mapper);

            var dataSave = await _repTransactionLocationTransaction.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<TransactionLocationTransactionDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionLocationTransactionDto>(result);

            return Ok(Result<TransactionLocationTransactionDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}
