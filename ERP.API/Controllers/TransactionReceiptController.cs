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
        public readonly IGenericRepository<Form> _RepForm;
        public TransactionReceiptController(IGenericRepository<TransactionReceipt>
            RepTransactionReceipt, IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<TransactionReceiptDetails> transactionReceiptDetails,
            IGenericRepository<Form> repForm,
            IGenericRepository<Transactions> repTransactions)
        {
            _RepForm = repForm;
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
        
        
        [HttpGet("GetByTransactionId")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await _RepTransactionReceiptDetallis.Find(x => x.TransactionsId == id)
                .Include(x => x.TransactionReceipt).ToListAsync();

            var mapperOut = _mapper.Map<TransactionReceiptDetailsDto[]>(DataSave);

            return Ok(Result<TransactionReceiptDetailsDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }  
        [HttpGet("GetTotalByTransactionId")]
        public async Task<IActionResult> GetTotalByTransactionId([FromQuery] Guid id)
        {
            var DataSave = await _RepTransactionReceiptDetallis.Find(x => x.TransactionsId == id)
                .ToListAsync();
            var total = DataSave.Sum(x => x.Paid); 
            return Ok(Result<decimal>.Success(total, MessageCodes.AllSuccessfully()));
        }
      
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionReceiptDto transactionReceipt)
        {

            var mapperIn = _mapper.Map<TransactionReceipt>(transactionReceipt);

            var ResultInsert = await _RepTransactionReceipt.InsertAsync(mapperIn);

            var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

            return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully())); ;


        }
        [HttpPost("CreateRecipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipePayDto data)
        {
            var transaccion = await RepTransactions.GetById((data.TransationId));
          
            Guid formId = Guid.Parse("41909373-7831-4e44-9380-efa5c96d22ba");
            TransactionReceipt receipt = new TransactionReceipt();
            var rowForm = await _RepForm.GetById(formId);
            if (rowForm.AllowSequence != null)
            {

                if (rowForm.AllowSequence.Value)
                {

                    rowForm.Sequence = rowForm.Sequence.Value + 1;
                    receipt.Document = rowForm.Prefix + rowForm.Sequence.ToString();

                }
            }
    
            receipt.Date = data.Date;
            receipt.Reference = data.Reference;
            receipt.BankId = data.BankId;
            receipt.ContactId = transaccion.ContactId.Value;
            receipt.PaymentMethodId = data.PaymentMethodId;
            receipt.CurrencyId = data.CurrencyId;
    
       
            try
            {
                _RepTransactionReceipt.Insert(receipt);
                await _RepTransactionReceipt.SaveChangesAsync();  
                TransactionReceiptDetails transactionReceiptDetails = new TransactionReceiptDetails();
                                                                            transactionReceiptDetails.TransactionsId = transaccion.Id;
                                                                            transactionReceiptDetails.TransactionReceiptId = receipt.Id;
                                                                            decimal forPay = 1 * data.Pay;
                                                                            transactionReceiptDetails.Paid = forPay;
                _RepTransactionReceiptDetallis.Insert(transactionReceiptDetails);
                await _RepTransactionReceiptDetallis.SaveChangesAsync();
      
                return Ok(Result<RecipePayDto>.Success(data, MessageCodes.AddedSuccessfully())); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating)); 

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
