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
    public class BankTransactionController : ControllerBase
    {
        private readonly IGenericRepository<BankTransaction> _repBankTransaction;

        private readonly IMapper _mapper;

        private int _dataSave;

        public BankTransactionController(IGenericRepository<BankTransaction> repBankTransaction, IMapper mapper)
        {
            _repBankTransaction = repBankTransaction;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankTransactionDto data)
        {

            var mapper = _mapper.Map<BankTransaction>(data);


            var result = await _repBankTransaction.InsertAsync(mapper);

            _dataSave = await _repBankTransaction.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<BankTransactionDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<BankTransactionDto>(result);
            return Ok(Result<BankTransactionDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repBankTransaction.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Bank).Include(x => x.PaymentMethod).ToListAsync();


            var mapperOut = _mapper.Map<BankTransactionDto[]>(dataSave);

            return Ok(Result<BankTransactionDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BankTransactionDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getBankTransaction = _repBankTransaction.Find(x => x.IsActive == true
            && (x.Bank.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Bank.AccountNumber.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Include(x => x.Bank).Include(x => x.PaymentMethod).ToList();

            int totalRecords = getBankTransaction.Count();
            var dataMaperOut = _mapper.Map<List<BankTransactionDto>>(getBankTransaction);

            var listBankTransaction = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<BankTransactionDto>>.Success(listBankTransaction);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repBankTransaction.GetById(id);

            var mapperOut = _mapper.Map<BankTransactionDto>(dataSave);

            return Ok(Result<BankTransactionDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repBankTransaction.GetById(id);

            data.IsActive = false;

            await _repBankTransaction.Update(data);

            var save = await _repBankTransaction.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<BankTransactionDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<BankTransactionDto>(data);

            return Ok(Result<BankTransactionDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankTransactionDto updateDto)
        {
            var mapper = _mapper.Map<BankTransaction>(updateDto);
            mapper.IsActive = true;
            var result = await _repBankTransaction.Update(mapper);

            var dataSave = await _repBankTransaction.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<BankTransactionDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<BankTransactionDto>(result);

            return Ok(Result<BankTransactionDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}