using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Implementations;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionReceiptController : ControllerBase
    {
        public readonly IGenericRepository<TransactionReceipt> _RepTransactionReceipt;
        public readonly IGenericRepository<TransactionReceiptDetails> _RepTransactionReceiptDetallis;
        public readonly IGenericRepository<Transactions> RepTransactions;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionReceiptController(IGenericRepository<TransactionReceipt>
            RepTransactionReceipt, IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<TransactionReceiptDetails> transactionReceiptDetails,
            IGenericRepository<Transactions> repTransactions)
        {
            RepTransactions = repTransactions;
            _RepTransactionReceipt = RepTransactionReceipt;
            _RepTransactionReceiptDetallis = transactionReceiptDetails;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet("GetAllByContact")]
        public async Task<IActionResult> GetAllByContact(Guid ContactId, Guid TransactionStatusId)
        {

            var ListTransactionByCliente = await RepTransactions.Find(x => x.ContactId == ContactId
            && x.IsActive == true && x.TransactionStatusId == TransactionStatusId)
                .Include(x => x.Contact).Include(s => s.TransactionStatus).ToListAsync();

            var mapperOut = _mapper.Map<List<TransactionsDto>>(ListTransactionByCliente);

            return Ok(Result<List<TransactionsDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));

        }
      
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionReceiptDto transactionReceipt)
        {

            var mapperIn = _mapper.Map<TransactionReceipt>(transactionReceipt);

            var ResultInsert = await _RepTransactionReceipt.InsertAsync(mapperIn);

            var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

            return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully())); ;


        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await _RepTransactionReceipt.GetById(id);

            Data.IsActive = false;

            await _RepTransactionReceipt.Update(Data);

            var save = await _RepTransactionReceipt.SaveChangesAsync();
            var TransactionReceiptDetallis = await _RepTransactionReceiptDetallis.Find(x => x.TransactionReceiptId == id
            && x.IsActive == true).ToListAsync();

            foreach (var item in TransactionReceiptDetallis)
            {
                item.IsActive = false;

                await _RepTransactionReceiptDetallis.Update(item);

            }
            var saveReceiptDetallis = await _RepTransactionReceiptDetallis.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ContactDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ContactDto>(Data);

            return Ok(Result<ContactDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }


  

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionReceiptDto _UpdateDto)
        {

            var mapperIn = _mapper.Map<TransactionReceipt>(_UpdateDto);

            var ResultInsert = await _RepTransactionReceipt.Update(mapperIn);

            var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

            return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully())); 

        }
    }
}
