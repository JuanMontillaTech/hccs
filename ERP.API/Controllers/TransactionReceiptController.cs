        using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionReceiptController : ControllerBase
{
    private readonly IGenericRepository<TransactionReceipt> _repTransactionReceipt;
    private readonly IGenericRepository<TransactionReceiptDetails> _repTransactionReceiptDetallis;
    private readonly IGenericRepository<Transactions> _repTransactions;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<Form> _repForm;
    private readonly IGenericRepository<Company> _repCompanys;
    private readonly Guid _statusComplete = Guid.Parse("85685d53-d6a6-4381-944b-965ed1187fbd");
    private readonly Guid _statusForPay = Guid.Parse("85685d53-d6a6-4381-944b-965ed1147fbc");

    public TransactionReceiptController(IGenericRepository<TransactionReceipt>
            repTransactionReceipt, IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IGenericRepository<TransactionReceiptDetails> transactionReceiptDetails,
        IGenericRepository<Form> repForm,
        IGenericRepository<Company> repCompanys,
        
        IGenericRepository<Transactions> repTransactions)
    {
        _repForm = repForm;
        _repTransactions = repTransactions;
        _repTransactionReceipt = repTransactionReceipt;
        _repTransactionReceiptDetallis = transactionReceiptDetails;
        _httpContextAccessor = httpContextAccessor;
        _repCompanys = repCompanys;
        _mapper = mapper;
    }

    [HttpGet($"GetAllByContact")]
    public async Task<IActionResult> GetAllByContact(Guid contactId, Guid transactionStatusId)
    {
        var listTransactionByCliente = await _repTransactions.Find(x => x.ContactId == contactId
                                                                       && x.IsActive == true &&
                                                                       x.TransactionStatusId == transactionStatusId)
            .Include(x => x.Contact).Include(s => s.TransactionStatus).ToListAsync();

        var mapperOut = _mapper.Map<List<TransactionsDto>>(listTransactionByCliente);

        return Ok(Result<List<TransactionsDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }


    [HttpGet($"GetByTransactionId")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var dataSave = await _repTransactionReceiptDetallis.Find(x =>
                x.TransactionsId == id & x.IsActive == true)
            .Include(x => x.TransactionReceipt).ToListAsync();

        var mapperOut = _mapper.Map<TransactionReceiptDetailsDto[]>(dataSave);

        return Ok(Result<TransactionReceiptDetailsDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }


    [HttpGet($"GetRecipeById")]
    public async Task<IActionResult> GetRecipeById([FromQuery] Guid id)
    {
        var transactionReceipt = await _repTransactionReceipt.Find(x => x.Id == id)
            .Include(x => x.Contact)
            .Include(x => x.Currency)
            .Include(x => x.Banks).Include(x => x.PaymentMethods).Include(x => x.Currency)
            .FirstAsync();
        var transactionReceiptData = await _repTransactionReceiptDetallis.Find(x => x.TransactionReceiptId == id)
            .Include(x => x.Transactions).ToListAsync();

        var companyFind = await _repCompanys.GetAll();

        var company = companyFind.FirstOrDefault() ?? throw new ArgumentNullException("CompanyFind.FirstOrDefault()");
        
        if (transactionReceipt != null)
            if (transactionReceiptData != null)
            {
           
                var forPrint = new TransactionReceiptOutDto
                {
                    CompanyId = company.Id,
                    TaxId = company.CompanyCode,
                    CompanyName = company.CompanyName,
                    CompanyAdress = company.Address,
                    CompanyPhones = company.Phones,
                    TransactionReceipt = _mapper.Map<TransactionReceiptDto>(transactionReceipt),
                    TransactionReceiptDetails =
                        _mapper.Map<List<TransactionReceiptDetailsDto>>(transactionReceiptData)
                };


                return Ok(Result<TransactionReceiptOutDto>.Success(forPrint, MessageCodes.AllSuccessfully()));
            }

        return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating));
    }

    [HttpGet($"GetTotalByTransactionId")]
    public async Task<IActionResult> GetTotalByTransactionId([FromQuery] Guid id)
    {
        var dataSave = await _repTransactionReceiptDetallis.Find(
                x => x.TransactionsId == id
                & x.IsActive == true
                )
            .ToListAsync();
        var total = dataSave.Sum(x => x.Paid);
        return Ok(Result<decimal>.Success(total, MessageCodes.AllSuccessfully()));
    }
    [HttpGet("GetFilter")]
    [ProducesResponseType(typeof(Result<ICollection<TransactionReceiptDto>>), (int)HttpStatusCode.OK)]
    public IActionResult GetFilter([FromQuery] PaginationFilter filter, DateTime dateStart,
    DateTime dateEnd, bool valideFilter)
    {
        List<TransactionReceipt> firtFilter;
        if (valideFilter)
        {
            var eDate = DateTime.Parse(dateEnd.ToString(CultureInfo.CurrentCulture)).AddDays(1);
            firtFilter = _repTransactionReceipt.Find(x => x.IsActive == true)
                .Include(s => s.Contact).Where(x =>
                     x.Document.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower())
                    || (x.Date == dateStart && x.Date <= eDate)).OrderByDescending(x => x.CreatedDate).Take(filter.PageSize)
                .ToList();
        }
        else
        {
            firtFilter = _repTransactionReceipt.Find(x => x.IsActive == true)
                .Include(x => x.PaymentMethods)
                .Include(s => s.Contact).Where(x =>
                    x.Document.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Reference.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower())
                ).OrderByDescending(x => x.CreatedDate).Take(filter.PageSize).ToList();
        }

        var totalRecords = firtFilter.Count();

        var dataMaperOut = _mapper.Map<List<TransactionReceipt>>(firtFilter);


        var getTransactionReceipt = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);

        var result = Result<PagesPagination<TransactionReceipt>>.Success(getTransactionReceipt);

        return Ok(result);
    }

    [HttpPost($"Create")]
    public async Task<IActionResult> Create([FromBody] TransactionReceiptDto transactionReceipt)
    {
        var mapperIn = _mapper.Map<TransactionReceipt>(transactionReceipt);

        var resultInsert = await _repTransactionReceipt.InsertAsync(mapperIn);

        var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

        return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        ;
    }

    [HttpPost($"CreateIncomeReceipt")]
    public async Task<IActionResult> CreateIncomeReceipt([FromBody] TransactionReceiptDto transactionReceipt)
    {
        //var transactions = new Transactions();
        //transactions.ContactId = Guid.Parse("7198850E-10BC-4DE2-B893-CD7E18D1C679");
        //transactions.TransactionsType = data.TransactionType;
        //transactions.GlobalTotal = data.Total;
        //transactions.GlobalDiscount = 0;
        //transactions.Date = DateTime.Now;
        //transactions.Commentary = data.Name;

        var mapperIn = _mapper.Map<TransactionReceipt>(transactionReceipt);

        var resultInsert = await _repTransactionReceipt.InsertAsync(mapperIn);

        var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

        return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        ;
    }


    [HttpPost($"CreateRecipe")]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipePayDto data)
    {
        var transaccion = await _repTransactions.GetById(data.TransationId);

        var formId = Guid.Parse("41909373-7831-4e44-9380-efa5c96d22ba");
        var receipt = new TransactionReceipt();
        var rowForm = await _repForm.GetById(formId);
        if (rowForm.AllowSequence != null)
            if (rowForm.AllowSequence.Value)
            {
                if (rowForm.Sequence != null)
                {
                    rowForm.Sequence = rowForm.Sequence.Value + 1;
                    receipt.Document = rowForm.Prefix + rowForm.Sequence;
                }
            }

        receipt.Date = data.Date;
        receipt.Reference = data.Reference;
        receipt.BankId = data.BankId;
        if (transaccion.ContactId != null) receipt.ContactId = transaccion.ContactId.Value;
        receipt.PaymentMethodId = data.PaymentMethodId;
        receipt.CurrencyId = data.CurrencyId;


        try
        {
            _repTransactionReceipt.Insert(receipt);
            await _repTransactionReceipt.SaveChangesAsync();
            var transactionReceiptDetails = new TransactionReceiptDetails
            {
                TransactionsId = transaccion.Id,
                TransactionReceiptId = receipt.Id
            };
            var forPay = 1 * data.Pay;
            transactionReceiptDetails.Paid = forPay;
            _repTransactionReceiptDetallis.Insert(transactionReceiptDetails);
            await _repTransactionReceiptDetallis.SaveChangesAsync();
            data.Id = receipt.Id;
            var recipeSave = await _repTransactionReceiptDetallis.Find(
                    x => x.TransactionsId == data.TransationId & x.IsActive == true)
                .Include(x => x.TransactionReceipt).ToListAsync();

            var totaGlobalPage = recipeSave.Sum(p => p.Paid);
            if (data.GlobalTotal ==  totaGlobalPage)
            {
                var transacionRow = await _repTransactions.GetById(data.TransationId);
                transacionRow.TransactionStatusId =   _statusComplete;
                await  _repTransactions.SaveChangesAsync();

            }
            
            return Ok(Result<RecipePayDto>.Success(data, MessageCodes.AddedSuccessfully()));
        }
        catch (Exception e)
        {
            return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating));
        }

        
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(  Guid id)
    {
        var data = await _repTransactionReceipt.GetById(id);
 
        data.IsActive = false;

        await _repTransactionReceipt.Update(data);

        var transactionReceiptDetallisRows =
            await _repTransactionReceiptDetallis.Find(x=> 
                x.TransactionReceiptId == id).ToListAsync();
        var transactionReceiptDetallisRow = transactionReceiptDetallisRows.FirstOrDefault();
        if (transactionReceiptDetallisRow != null)
        {
            transactionReceiptDetallisRow.IsActive = false;
            await _repTransactionReceiptDetallis.SaveChangesAsync();
           
            var recipeSave = await _repTransactionReceiptDetallis.Find(
                    x => x.TransactionsId == transactionReceiptDetallisRow.TransactionsId & x.IsActive == true)
                .Include(x => x.TransactionReceipt).ToListAsync();

            var totaGlobalPage = recipeSave.Sum(p => p.Paid);
            var transacionRow = await _repTransactions.GetById(transactionReceiptDetallisRow.TransactionsId );

            if (transacionRow.GlobalTotal > totaGlobalPage)
            {
                transacionRow.TransactionStatusId = _statusForPay;
                await _repTransactions.SaveChangesAsync();
            }
        }

await        _repTransactionReceipt.SaveChangesAsync();
         
        var mapperOut = _mapper.Map<TransactionReceiptDto>(data);

        return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
    }


    [HttpPut($"Update")]
    public async Task<IActionResult> Update([FromBody] TransactionReceiptDto updateDto)
    {
        var mapperIn = _mapper.Map<TransactionReceipt>(updateDto);

        var resultInsert = await _repTransactionReceipt.Update(mapperIn);

        var mapperOut = _mapper.Map<TransactionReceiptDto>(mapperIn);

        return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }
}