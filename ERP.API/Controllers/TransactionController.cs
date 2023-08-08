using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using System.Net;

namespace ERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IGenericRepository<Transactions> _repTransactionss;
    private readonly IGenericRepository<TransactionsDetails> _repTransactionssDetails;
    private readonly IGenericRepository<TransactionLocationTransaction> _repTransactionLocationTransaction;
    private readonly IGenericRepository<TransactionsDetailsElement> _reTransactionsDetailsElement;
    private readonly IGenericRepository<Contact> _repContacts;
    private readonly IGenericRepository<Concept> _repConcept;
    private readonly IGenericRepository<Company> _repCompanys;
    private readonly IMapper _mapper;

    private readonly ITransactionService _transactionService;

    public TransactionController(
        IGenericRepository<Transactions> repTransactionss,
        IGenericRepository<TransactionsDetails> repTransactionssDetails,
        IGenericRepository<Concept> repConcept,
        IGenericRepository<Company> repCompanys,
        IGenericRepository<Contact> repContacts,
        IGenericRepository<TransactionLocationTransaction> repTransactionLocationTransaction,
        IGenericRepository<TransactionsDetailsElement> reTransactionsDetailsElement,
        IMapper mapper,
        ITransactionService transactionService)
    {
        _repCompanys = repCompanys;
        _repContacts = repContacts;
        _repConcept = repConcept;
        _repTransactionss = repTransactionss;
        _repTransactionssDetails = repTransactionssDetails;
        _transactionService = transactionService;
        _mapper = mapper;
        _repTransactionLocationTransaction = repTransactionLocationTransaction;
        _reTransactionsDetailsElement = reTransactionsDetailsElement;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TransactionsDto data)
    {
        try
        {
            var mapperIn = _mapper.Map<Transactions>(data);
            var result = await _transactionService.TransactionProcess(mapperIn, data.FormId);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }
        catch (Exception ex)
        {
            return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
        }
    }

    [HttpPost("ProccesLocation/{id}/{PaymentMethodId}/{Contactid}/{TaxContactNumber}")]
    public async Task<IActionResult> ProccesLocation(Guid id, Guid PaymentMethodId, Guid Contactid,
        string? TaxContactNumber)
    {
        var formId = Guid.Parse("25f94e8c-8ea0-4ee0-adf5-02149a0e080b");
        try
        {
            var transactions = new Transactions();
            transactions.Date = DateTime.Now;
            transactions.TransactionsType = 6;
            transactions.ContactId = Contactid;
            if (TaxContactNumber != null)
                transactions.TaxContactNumber = TaxContactNumber.Replace($".", string.Empty).Trim();
            transactions.PaymentMethodId = PaymentMethodId;
            //Todo: tengo que crear el cliente y generar el codigo de factura
            var transactionsDetails = new List<TransactionsDetails>();
            decimal bigTotal = 0;
            var allLocationTransaction = await _repTransactionLocationTransaction
                .Find(x => x.IsActive == true && x.TransactionLocationId == id)
                .AsQueryable().Include(x => x.Transactions).ThenInclude(x => x.TransactionsDetails).ToListAsync();
            foreach (var item in allLocationTransaction)
            {
                item.IsActive = false;

                //Cambiar orden a completado
                var statusComple = Guid.Parse("85685D53-D6A6-4381-944B-995ED2667FBA");
                var Invoice = await _repTransactionss.GetById(item.TransactionId);
                Invoice.TransactionStatusId = statusComple;
                await _repTransactionss.Update(Invoice);
                //Agrego las transacciones.
                foreach (var itemDetails in item.Transactions.TransactionsDetails)
                {
                    var mapperIn = _mapper.Map<TransactionsDetailsDto>(itemDetails);
                    mapperIn.Id = Guid.Empty;


                    transactionsDetails.Add(_mapper.Map<TransactionsDetails>(mapperIn));
                }

                bigTotal = bigTotal + item.Transactions.GlobalTotal;


                await _repTransactionLocationTransaction.SaveChangesAsync();
            }

            var result = await _repTransactionss.SaveChangesAsync();
            if (transactionsDetails.Count > 0)
            {
                transactions.GlobalTotal = bigTotal;
                transactions.TransactionsDetails = transactionsDetails;
                await _transactionService.TransactionProcess(transactions, formId);
            }

            return Ok(Result<TransactionsDto>.Success(_mapper.Map<TransactionsDto>(transactions),
                MessageCodes.AddedSuccessfully()));
        }
        catch (Exception ex)
        {
            return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
        }
    }

    [HttpPost("CreatePost")]
    public async Task<IActionResult> CreatePost([FromBody] PosDto data)
    {
        var transactions = new Transactions();
        transactions.ContactId = Guid.Parse("7198850E-10BC-4DE2-B893-CD7E18D1C679");
        transactions.TransactionsType = data.TransactionType;
        transactions.GlobalTotal = data.Total;
        transactions.GlobalDiscount = 0;
        transactions.Date = DateTime.Now;
        transactions.Commentary = data.Name;

        var listtransactionsDetails = new List<TransactionsDetails>();
        foreach (var item in data.TransactionsItems)
        {
            var transactionsDetails = new TransactionsDetails();
            var concept = await _repConcept.GetById(item.Id);
            decimal totalSet = 0;

            if (concept.PriceSale != null)
            {
                totalSet = item.Count * concept.PriceSale.Value;
                transactionsDetails.TransactionsId = transactions.Id;
                transactionsDetails.ReferenceId = concept.Id;
                transactionsDetails.Description = concept.Reference;
                transactionsDetails.Amount = item.Count;
                transactionsDetails.Price = concept.PriceSale.Value;
            }

            transactionsDetails.Discount = 0;
            transactionsDetails.Tax = 0;
            transactionsDetails.Total = totalSet;
            var listTransactionsDetailsElement =
                new List<TransactionsDetailsElement>();
            if (item.ElementConcept != null)
                foreach (var itemElement in item.ElementConcept)
                    if (itemElement.IsActive == false)
                    {
                        var transactionsDetailsElement =
                            new TransactionsDetailsElement
                            {
                                TransactionsDetailsId = transactionsDetails.Id,
                                ReferenceId = itemElement.ConceptId,
                                Detaills = itemElement.Name
                            };
                        listTransactionsDetailsElement.Add(transactionsDetailsElement);
                    }

            transactionsDetails.TransactionsDetailsElement = listTransactionsDetailsElement;


            listtransactionsDetails.Add(transactionsDetails);
        }

        transactions.TransactionsDetails = listtransactionsDetails;

        var result = await _transactionService.TransactionProcess(transactions, data.FormId);

        var transactionLocationTransaction = new TransactionLocationTransaction();

        transactionLocationTransaction.TransactionId = result.Id;

        transactionLocationTransaction.TransactionLocationId = data.TransactionLocationId;


        await _repTransactionLocationTransaction.InsertAsync(transactionLocationTransaction);

        await _repTransactionLocationTransaction.SaveChangesAsync();

        var mapperOut = _mapper.Map<TransactionsDto>(result);
        return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
    }

    [HttpPost("CreateChangeTypes")]
    public async Task<IActionResult> CreateChangeTypes([FromBody] int TransactionsType, Guid FormId,
        Guid TransactionsId)
    {
        try
        {
            var DataSave = await _repTransactionss.GetById(TransactionsId);

            DataSave.TransactionsType = TransactionsType;

            var result = await _transactionService.TransactionProcess(DataSave, FormId);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }
        catch (Exception ex)
        {
            return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
        }
    }

    [HttpPost("CreateContact")]
    public async Task<IActionResult> CreateContact([FromBody] TransactionsContactDto data)
    {
        try
        {
            var mapperIn = _mapper.Map<Transactions>(data);
            var mapperContact = _mapper.Map<Contact>(data);
            var resultContact = await _repContacts.InsertAsync(mapperContact);
            var DataSave = await _repContacts.SaveChangesAsync();

            mapperIn.ContactId = resultContact.Id;
            var result = await _transactionService.TransactionProcess(mapperIn, data.FormId.Value);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }
        catch (Exception ex)
        {
            return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
        }
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var dataSave = await _repTransactionss.GetAll();
        var dataSaveDetails = await _repTransactionssDetails.GetAll();
        var dataFillter = dataSave.Where(x => x.IsActive == true).OrderByDescending(x => x.Date).ToList();
        foreach (var item in dataFillter)
            item.TransactionsDetails = dataSaveDetails.AsQueryable()
                .Where(x => x.IsActive == true && x.TransactionsId == item.Id).ToList();
        return Ok(Result<IEnumerable<Transactions>>.Success(dataFillter, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetBoxClose")]
    public async Task<IActionResult> GetBoxClose([FromQuery] DateTime startDate, DateTime endDate, int TransationType)
    {
        var query = await _repTransactionss.Find(x => (x.IsActive == true)
            & (x.CreatedDate > startDate) && (x.CreatedDate < endDate)
            & (x.TransactionsType == TransationType)).Include(x => x.PaymentMethods).ToListAsync();
        return Ok(Result<IEnumerable<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetAllByContact")]
    public async Task<IActionResult> GetAllByContact(Guid ContactId)
    {
        var query = await _repTransactionss.Find(x => x.ContactId == ContactId && x.IsActive == true)
            .Include(x => x.Contact).Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails)
            .ThenInclude(X => X.Concept).FirstOrDefaultAsync();

        var data = query.TransactionsDetails.Where(x => x.IsActive = true).ToList();
        query.TransactionsDetails = data;

        return Ok(Result<Transactions>.Success(query, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        try
        {
            var DataSave = await _repTransactionss.Find(x => x.Id == id
                )
                .Include(x => x.Contact)
                .Include(x => x.TransactionsDetails).ThenInclude(x => x.Concept)
                .FirstOrDefaultAsync();
            var DataOutNull = DataSave.TransactionsDetails.
                Where(t => t.ReferenceId != null)
                .ToList();
            DataSave.TransactionsDetails = DataOutNull;
            var mapperOut = _mapper.Map<TransactionsDto>(DataSave);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        catch (Exception ex)
        {
            return Ok(Result<TransactionsDto>.Fail("No tiene registros", MessageCodes.BabData()));
        }
    }

    [HttpGet("GetTicket")]
    public async Task<IActionResult> GetTicket([FromQuery] Guid id)
    {
        Transactions invoice;
        invoice = await _repTransactionss.Find(x => x.Id == id)
            .Include(x => x.Contact)
            .Include(x => x.PaymentTerms)
            .Include(x => x.PaymentMethods)
            .FirstOrDefaultAsync();

        var invocieDetails = await _repTransactionssDetails.Find(x => x.IsActive == true
                                                                      && x.TransactionsId == id).Include(x => x.Concept)
            .ToListAsync();

        if (invocieDetails.Count > 0)
            invoice.TransactionsDetails = invocieDetails;

        if (invoice != null)
        {
            var companyFind = await _repCompanys.GetAll();

            var company = companyFind.FirstOrDefault();

            var ticket = new TicketDto();

            if (company != null)
            {
                ticket.CompanyId = company.Id;

                ticket.TaxId = company.CompanyCode;

                ticket.CompanyName = company.CompanyName;
                ticket.TaxNumber = invoice.TaxNumber;

                ticket.TaxContactNumber = invoice.Contact.DocumentNumber;

                ticket.CompanyAdress = company.Address;

                ticket.CompanyPhones = company.Phones;
            }

            ticket.InvoiceId = invoice.Id;

            ticket.InvoiceCode = invoice.Code;

            ticket.InvoiceDate = invoice.Date;

            ticket.InvoiceComentary = invoice.Commentary;

            ticket.TotalAmount = invoice.TotalAmount;
            ticket.TotalAmountTax = invoice.TotalAmountTax;
            
            
            ticket.InvoiceTotalTax = invoice.TotalTax;
            
            

            ticket.InvoiceTotal = invoice.GlobalTotal;
            ticket.GlobalTotalTax = invoice.GlobalTotalTax;
            
            

            if (invoice.PaymentTermId != null)
            {
                ticket.InvoicePaymentTermId = invoice.PaymentTermId;

                ticket.InvoicePaymentTerm =
                    invoice.PaymentTerms != null ? invoice.PaymentTerms.Name : "Terminos no encontrado";
            }

            if (invoice.PaymentMethodId != null)
            {
                ticket.InvoicePaymentMethodId = invoice.PaymentMethodId;

                ticket.InvoicePaymentMethod = invoice.PaymentMethods != null
                    ? invoice.PaymentMethods.Name
                    : "Metodo no encontrado";
            }

            if (invoice.ContactId != null)
            {
                ticket.InvoiceContactId = invoice.ContactId;

                ticket.InvoiceContactName = invoice.Contact.Name;

                ticket.InvoiceContactPhone = invoice.Contact.Phone1 + " " + invoice.Contact.Phone2 + " " +
                                             invoice.Contact.CellPhone;

                ticket.InvoiceContactAdress = invoice.Contact.Address;

                ticket.InvoiceContactTaxId = invoice.Contact.DocumentNumber;
            }

            if (invoice.TransactionsDetails.Count > 0)
            {
                var listTicketDetallis = new List<TicketDetallisDto>();

                foreach (var invoiceDetallisRow in invoice.TransactionsDetails)
                {
                    var concep = await _repConcept.GetById(invoiceDetallisRow.ReferenceId);
                    if (concep != null)
                    {
                        var ticketDetallis = new TicketDetallisDto();


                        ticketDetallis.ReferenceId = concep.Id;

                        ticketDetallis.Total = invoiceDetallisRow.Total;
                        ticketDetallis.PriceWithTax = invoiceDetallisRow.PriceWithTax;
                        ticketDetallis.TotalTax = invoiceDetallisRow.TotalTax;

                        ticketDetallis.Price = invoiceDetallisRow.Price;

                        ticketDetallis.Amount = invoiceDetallisRow.Amount;

                        ticketDetallis.Reference = invoiceDetallisRow.Concept.Reference;

                        ticketDetallis.Description = invoiceDetallisRow.Concept.Description;

                        listTicketDetallis.Add(ticketDetallis);
                    }
                }

                if (listTicketDetallis.Count > 0) ticket.TicketDetallisDtos = listTicketDetallis;
            }

            return Ok(Result<TicketDto>.Success(ticket, MessageCodes.AllSuccessfully()));
        }

        return Ok(Result<TicketDetallisDto>.Fail("No tiene registros", MessageCodes.BabData()));
    }

    [HttpGet("SetStatus")]
    public async Task<IActionResult> SetStatus([FromQuery] Guid transactionId, Guid transactionStatusId)
    {
        try
        {
            var invoice = await _repTransactionss.GetById(transactionId);

            invoice.TransactionStatusId = transactionStatusId;

            await _repTransactionss.Update(invoice);
            var result = await _repTransactionss.SaveChangesAsync();
            return Ok(Result<int>.Success(result, MessageCodes.AllSuccessfully()));
        }
        catch (Exception)
        {
            return Ok(Result<TicketDetallisDto>.Fail("No tiene registros", MessageCodes.BabData()));
        }
    }

    [HttpGet("GetAllDataById")]
    public async Task<IActionResult> GetAllDataById([FromQuery] Guid id)
    {
        var query = await _repTransactionss.Find(x => x.Id == id).Include(x => x.Contact)
            .Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept)
            .FirstOrDefaultAsync();


        return Ok(Result<Transactions>.Success(query, MessageCodes.AllSuccessfully()));
    }


    [HttpGet("GetAllByType")]
    public async Task<IActionResult> GetAllByType([FromQuery] int TransactionsTypeId)
    {
        var query = await _repTransactionss.Find(x => x.TransactionsType == TransactionsTypeId)
            .Where(x => x.IsActive == true).Include(x => x.Contact).Include(x => x.PaymentMethods)
            .Include(x => x.PaymentTerms).Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails)
            .ThenInclude(X => X.Concept)
            .OrderByDescending(x => x.Date).ToListAsync();
        return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetAllByTypeStatus")]
    public async Task<IActionResult> GetAllByTypeStatus([FromQuery] int TransactionsTypeId)
    {
        var send = Guid.Parse("85685D53-D6A6-4381-944B-995ED2667FBA");
        var sendComplete = Guid.Parse("85685D53-D6A6-4381-944B-995ED1187FBA");
        var query = await _repTransactionss.Find(x => (x.TransactionsType ==
                TransactionsTypeId && x.TransactionStatusId == send) || x.TransactionStatusId == sendComplete)
            .Where(x => x.IsActive == true).Include(x => x.Contact).Include(x => x.PaymentMethods)
            .Include(x => x.PaymentTerms).Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails)
            .ThenInclude(X => X.Concept)
            .OrderBy(x => x.Date).ToListAsync();


        var DataMaperOut = _mapper.Map<List<TransactionsDto>>(query);
        foreach (var Transactionsitem in DataMaperOut)
        foreach (var TransactionsDetailsitem in Transactionsitem.TransactionsDetails)
        {
            var QueryDetailsElement = await _reTransactionsDetailsElement
                .Find(x => x.TransactionsDetailsId == TransactionsDetailsitem.Id).Where(x => x.IsActive == true)
                .ToListAsync();
            if (QueryDetailsElement.Count > 0)
                TransactionsDetailsitem.TransactionsDetailsElement =
                    _mapper.Map<List<TransactionsDetailsElementDto>>(QueryDetailsElement);
        }


        return Ok(Result<List<TransactionsDto>>.Success(DataMaperOut, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetAllByTypeStatusIsService")]
    public async Task<IActionResult> GetAllByTypeStatusIsService([FromQuery] int TransactionsTypeId,
        Guid TransactionStatusId)
    {
        var query = await _repTransactionss.Find(x => x.TransactionsType ==
                TransactionsTypeId && x.TransactionStatusId == TransactionStatusId).Where(x => x.IsActive == true)
            .Include(x => x.Contact).Include(x => x.PaymentMethods).Include(x => x.PaymentTerms)
            .Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept)
            .OrderBy(x => x.Date).ToListAsync();

        var result = new List<Transactions>();

        foreach (var transaction in query)
        {
            var isValide = false;
            foreach (var item in transaction.TransactionsDetails)
                if (item.Concept.IsServicie == true)
                {
                    var isLocation = await _repTransactionLocationTransaction
                        .Find(x => x.TransactionId == transaction.Id).FirstOrDefaultAsync();
                    isValide = isLocation.IsActive;
                }

            if (isValide) result.Add(transaction);
        }

        var dataMaperOut = _mapper.Map<List<TransactionsDto>>(result);
        foreach (var transactionsitem in dataMaperOut)
        foreach (var transactionsDetailsitem in transactionsitem.TransactionsDetails)
        {
            var queryDetailsElement = await _reTransactionsDetailsElement
                .Find(x => x.TransactionsDetailsId == transactionsDetailsitem.Id).Where(x => x.IsActive == true)
                .ToListAsync();
            if (queryDetailsElement.Count > 0)
                transactionsDetailsitem.TransactionsDetailsElement =
                    _mapper.Map<List<TransactionsDetailsElementDto>>(queryDetailsElement);
        }

        return Ok(Result<List<TransactionsDto>>.Success(dataMaperOut, MessageCodes.AllSuccessfully()));
    }


    [HttpGet("GetFilter")]
    [ProducesResponseType(typeof(Result<ICollection<TransactionsDto>>), (int)HttpStatusCode.OK)]
    public IActionResult GetFilter([FromQuery] PaginationFilter filter, int transactionsTypeId, DateTime dateStart,
        DateTime dateEnd, bool valideFilter)
    {
        List<Transactions> firtFilter;
        if (valideFilter)
        {
            var eDate = DateTime.Parse(dateEnd.ToString(CultureInfo.CurrentCulture)).AddDays(1); 
            firtFilter = _repTransactionss.Find(x => x.IsActive == true && x.TransactionsType == transactionsTypeId)
                .Include(x => x.PaymentMethods).Include(x => x.PaymentTerms).Include(s => s.TransactionStatus)
                .Include(s => s.Contact).Include(x => x.TransactionsDetails).Where(x =>
                    x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Reference.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower())
                    || (x.Date == dateStart && x.Date <= eDate)).OrderByDescending(x => x.CreatedDate).Take(filter.PageSize)
                .ToList();
        }
        else
        {
            firtFilter = _repTransactionss.Find(x => x.IsActive == true && x.TransactionsType == transactionsTypeId)
                .Include(x => x.PaymentMethods).Include(x => x.PaymentTerms).Include(s => s.TransactionStatus)
                .Include(s => s.Contact).Include(x => x.TransactionsDetails).Where(x =>
                    x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Reference.ToLower().Contains(filter.Search.Trim().ToLower())
                    || x.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower())
                ).OrderByDescending(x => x.CreatedDate).Take(filter.PageSize).ToList();
        }

        var totalRecords = firtFilter.Count();

        var dataMaperOut = _mapper.Map<List<TransactionsDto>>(firtFilter);


        var getTransasaction = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);

        var result = Result<PagesPagination<TransactionsDto>>.Success(getTransasaction);

        return Ok(result);
    }

    [HttpGet("GetAllByTypeDisableRow")]
    public async Task<IActionResult> GetAllByTypeDisableRow([FromQuery] int transactionsTypeId)
    {
        var query = await _repTransactionss.Find(x => x.TransactionsType == transactionsTypeId)
            .Where(x => x.IsActive == false).Include(x => x.Contact).Include(x => x.PaymentMethods)
            .Include(x => x.PaymentTerms).Include(s => s.TransactionStatus).Include(x => x.TransactionsDetails)
            .ThenInclude(x => x.Concept).ToListAsync();

        return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
    }


    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var re = Request;
        var headers = re.Headers;
        string token = Request.Headers["Authorization"];
        var data = await _repTransactionss.GetById(id);

        data.IsActive = false;

        await _repTransactionss.Update(data);

        var save = await _repTransactionss.SaveChangesAsync();

        if (save != 1)
            return Ok(Result<TransactionsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

        var mapperOut = _mapper.Map<TransactionsDto>(data);

        return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
    }

    [HttpDelete("TransactionssDetailsDelete/{id}")]
    public async Task<IActionResult> TransactionssDetailsDelete(Guid id)
    {
        var re = Request;
        var headers = re.Headers;
        string Token = Request.Headers["Authorization"];
        var Data = await _repTransactionssDetails.GetById(id);

        Data.IsActive = false;

        await _repTransactionssDetails.Update(Data);

        var save = await _repTransactionssDetails.SaveChangesAsync();

        if (save != 1)
            return Ok(Result<TransactionsDetailsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

        var mapperOut = _mapper.Map<TransactionsDetailsDto>(Data);

        return Ok(Result<TransactionsDetailsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] TransactionsDto _UpdateDto)
    {
        var mapperIn = _mapper.Map<Transactions>(_UpdateDto);
        var result = await _transactionService.TransactionProcess(mapperIn, _UpdateDto.FormId);
        var mapperOut = _mapper.Map<TransactionsDto>(result);
        return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
    }
}