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
    private readonly IGenericRepository<FormLedgerAccount> _repFormLedger;
    private readonly IMapper _mapper;
    private readonly ISysRepository<Form> RepForms;
    private readonly IGenericRepository<Company> _repCompanys;
 
    private readonly ITransactionService transactionService;
    public TransactionReceiptController(IGenericRepository<TransactionReceipt>
            repTransactionReceipt, IMapper mapper, ISysRepository<Form> RepForms,IGenericRepository<FormLedgerAccount> _repFormLedger,
         
        IGenericRepository<TransactionReceiptDetails> transactionReceiptDetails,
        
        IGenericRepository<Company> repCompanys,

        IGenericRepository<Transactions> repTransactions, ITransactionService transactionService)
    {

        this.RepForms = RepForms;
        _repTransactions = repTransactions;
        _repTransactionReceipt = repTransactionReceipt;
        _repTransactionReceiptDetallis = transactionReceiptDetails;
        this._repFormLedger = _repFormLedger;
        _repCompanys = repCompanys;
        _mapper = mapper;
        this.transactionService = transactionService;
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
                 x.IsActive == true)
            .Include(x => x.TransactionReceipt).ToListAsync();

        var mapperOut = _mapper.Map<TransactionReceiptDetailsDto[]>(dataSave);

        return Ok(Result<TransactionReceiptDetailsDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }

   [HttpGet($"GetRecipeByIdForPrint")]
public async Task<IActionResult> GetRecipeByIdForPrint([FromQuery] Guid id)
{
    try
    {
        // 1. Optimización de Includes
        var transactionReceipt = await _repTransactionReceipt.Find(x => x.Id == id)
            .Include(x => x.Contact)
            .Include(x => x.TransactionReceiptDetails)
            .Include(x => x.Box)
            .Include(x => x.PaymentMethods)
            .Include(x => x.Currency)
            .FirstAsync();

        // 2. Optimización de la búsqueda de la compañía
        var company = await _repCompanys.FirstOrDefaultAsync() 
                       ?? throw new ArgumentNullException("Company not found");

        // 3. Optimización de la búsqueda del formulario
        var dataSaveForm = await RepForms.Find(x => x.IsActive && x.TransactionsType == transactionReceipt.Type).FirstOrDefaultAsync();

        // 4. Optimización de la búsqueda y mapeo de dataFormLedger
        var dataFormLedger = await _repFormLedger
            .Find(x => x.IsActive && x.FormId == dataSaveForm.Id)
            .Include(x => x.LedgerAccount)
            .OrderByDescending(x => x.Index).ToListAsync();

        var ListtransactionExt = dataFormLedger
            .Select(rowFormLedger => 
            {
                var totalAmount = transactionReceipt.TransactionReceiptDetails
                    .FirstOrDefault(x => x.referenceId == rowFormLedger.LedgerAccountId)?.Paid;

                return totalAmount.HasValue ? new TransactionExt
                {
                    Value = totalAmount.Value,
                    Index = rowFormLedger.Index,
                    Label = rowFormLedger.LedgerAccount.Name
                } : null;
            })
            .Where(x => x != null)
            .ToList();

        // 5. Construcción del DTO
        var forPrint = new TransactionReceiptOutDto
        {
            CompanyId = company.Id,
            TaxId = company.CompanyCode,
            CompanyName = company.CompanyName,
            CompanyAdress = company.Address,
            CompanyPhones = company.Phones,
            TransactionReceipt = _mapper.Map<TransactionReceiptDto>(transactionReceipt),
            TransactionExt = ListtransactionExt
        };

        return Ok(Result<TransactionReceiptOutDto>.Success(forPrint, MessageCodes.AllSuccessfully()));
    }
    catch (Exception ex)
    {
        // 6. Manejo de excepciones (considera loguear la excepción)
        return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating)); 
    }
}
    
    


    [HttpGet($"GetRecipeById")]
    public async Task<IActionResult> GetRecipeById([FromQuery] Guid id)
    {
        try
        {

            var transactionReceipt = await _repTransactionReceipt.Find(x => x.Id == id)
                .Include(x => x.Contact) 
                 .Include(x=> x.TransactionReceiptDetails)
                 .FirstAsync();
        
         
         var mapperOut = _mapper.Map<TransactionReceiptDto>(transactionReceipt);

         return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.AllSuccessfully())); 
        }
        catch (Exception )
        {
            return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating));
        }
    }


    [HttpGet("GetFilter")]
    [ProducesResponseType(typeof(Result<ICollection<TransactionReceiptDto>>), (int)HttpStatusCode.OK)]
    public IActionResult GetFilter([FromQuery] PaginationFilter filter, DateTime dateStart,
    DateTime dateEnd, bool valideFilter, int typeTransaction)
    {
          var getBanks = _repTransactionReceipt.Find(x => x.IsActive == true && x.Type == typeTransaction
            ).Include(x=> x.Contact).ToList();

            int totalRecords = getBanks.Count();
            var dataMaperOut = _mapper.Map<List<TransactionReceiptDto>>(getBanks);

            var listBanks = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords) ;
            var result = Result<PagesPagination<TransactionReceiptDto>>.Success(listBanks);
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


    [HttpPost($"CreateRecipe")]
    public async Task<IActionResult> CreateRecipe([FromBody] RecipePayDto data)
    {
        try
        {

            await transactionService.TransactionReceiptProcess(data);
            return Ok(Result<RecipePayDto>.Success(data, MessageCodes.AddedSuccessfully()));
        }
        catch (Exception e)
        {
    return Ok(Result<bool>.Success(false, MessageCodes.ErrorCreating  + e.Message));
        }
        

    }

     


    [HttpPut($"Update")]
    public async Task<IActionResult> Update([FromBody] RecipePayDto data)
    {
         
        await transactionService.TransactionReceiptProcess(data);
        return Ok(Result<RecipePayDto>.Success(data, MessageCodes.AddedSuccessfully()));
 
    }
    
    
    [HttpDelete("Delete/{id}")]

    public async Task<IActionResult> Delete(Guid id)
    {
        var Data = await _repTransactionReceipt.GetById(id);

        Data.IsActive = false;

        await _repTransactionReceipt.Update(Data);

        var save = await _repTransactionReceipt.SaveChangesAsync();

        if (save != 1)
            return Ok(Result<TransactionReceiptDto>.Fail(MessageCodes.ErrorDeleting, "API"));

        var mapperOut = _mapper.Map<TransactionReceiptDto>(Data);

        return Ok(Result<TransactionReceiptDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
    }
}