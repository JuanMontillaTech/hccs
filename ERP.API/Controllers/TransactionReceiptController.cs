using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
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
        public readonly IGenericRepository<Transactions> RepTransactions;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionReceiptController(IGenericRepository<TransactionReceipt>
            RepTransactionReceipt, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IGenericRepository<Transactions> repTransactions)
        {
            RepTransactions = repTransactions;
            _RepTransactionReceipt = RepTransactionReceipt;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet("GetAllByContact")]
        public async Task<IActionResult> GetAllByContact(Guid ContactId, Guid TransactionStatusId)
        {

            var ListTransactionByCliente = await RepTransactions.Find(x => x.ContactId == ContactId
            && x.IsActive == true
            && x.TransactionStatusId == TransactionStatusId
            ).
                Include(x => x.Contact).
                    Include(s => s.TransactionStatus).ToListAsync();
            List<TransactionReceiptDetailsDto> ListTransactions = new List<TransactionReceiptDetailsDto>();


            foreach (var Transaction in ListTransactionByCliente)
            {
                var ListReceiptPaid = await _RepTransactionReceipt.
                       Find(x => x.TransactionsId == Transaction.Id && x.IsActive == true)
                        .Include(x => x.Transactions).ToListAsync();

                decimal Paid = ListReceiptPaid.Sum(x => x.Paid);
                TransactionReceiptDetailsDto transactionReceiptDetailsDto = new();
                transactionReceiptDetailsDto.IsActive = true;
                transactionReceiptDetailsDto.Paid = ListReceiptPaid.Count > 0 ? Transaction.GlobalTotal - Paid : Transaction.GlobalTotal;
                transactionReceiptDetailsDto.ForPaid = Transaction.GlobalTotal;
                transactionReceiptDetailsDto.Received = 0;
                transactionReceiptDetailsDto.Code = Transaction.Code;
                transactionReceiptDetailsDto.TransactionsId = Transaction.Id;
                ListTransactions.Add(transactionReceiptDetailsDto);




            }


            return Ok(Result<List<TransactionReceiptDetailsDto>>.Success(ListTransactions, MessageCodes.AllSuccessfully()));

        }


    }
}
