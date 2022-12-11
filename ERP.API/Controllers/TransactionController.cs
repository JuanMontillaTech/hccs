
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ERP.Services.Implementations;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using System.Net;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly IGenericRepository<Transactions> RepTransactionss;
        public readonly IGenericRepository<TransactionsDetails> RepTransactionssDetails;
        public readonly IGenericRepository<TransactionLocationTransaction> RepTransactionLocationTransaction;
        public readonly IGenericRepository<TransactionsDetailsElement> ReTransactionsDetailsElement;
        public readonly IGenericRepository<Contact> RepContacts;
        public readonly INumerationService numerationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IGenericRepository<Journal> RepJournals;
        public readonly IGenericRepository<JournaDetails> RepJournalsDetails;
        public readonly IGenericRepository<Concept> RepConcept;
        public readonly IGenericRepository<Company> RepCompanys;
        private readonly IMapper _mapper;

        private readonly ITransactionService TransactionService;

        public TransactionController(
        IGenericRepository<Transactions> repTransactionss,
        IGenericRepository<TransactionsDetails> repTransactionssDetails, 
        IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails,
        IGenericRepository<Concept> _RepConcept,
        IGenericRepository<Company> repCompanys,
        IGenericRepository<Contact> _RepContacts,
        IGenericRepository<TransactionLocationTransaction> _RepTransactionLocationTransaction,
        IGenericRepository<TransactionsDetailsElement> _ReTransactionsDetailsElement,
        INumerationService numerationService,
        IMapper mapper, 
        IHttpContextAccessor httpContextAccessor,
        ITransactionService transactionService)
        {
            RepJournals = repJournals;
            RepCompanys = repCompanys;
            RepContacts = _RepContacts;
            RepConcept = _RepConcept;
            RepJournalsDetails = repJournalsDetails;
            this.numerationService = numerationService;
            RepTransactionss = repTransactionss;
            RepTransactionssDetails = repTransactionssDetails;
            _httpContextAccessor = httpContextAccessor;
            TransactionService = transactionService;
            _mapper = mapper;
            RepTransactionLocationTransaction = _RepTransactionLocationTransaction;
            ReTransactionsDetailsElement = _ReTransactionsDetailsElement;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDto data)
        {
            try
            {
                var mapperIn = _mapper.Map<Transactions>(data);
                var result = await TransactionService.TransactionProcess(mapperIn, data.FormId);
                var mapperOut = _mapper.Map<TransactionsDto>(result);
                return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));

            }
            catch (Exception ex)
            {

                return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
            }

        }
        [HttpPost("ProccesLocation/{id}/{PaymentMethodId}")]
        public async Task<IActionResult> ProccesLocation(  Guid id , Guid PaymentMethodId)
        {
            try
            {
                Transactions transactions = new Transactions();
                transactions.Date = DateTime.Now;
                transactions.TransactionsType = 6;
                Guid FormId = Guid.Parse("25f94e8c-8ea0-4ee0-adf5-02149a0e080b");
                transactions.PaymentMethodId = PaymentMethodId;
                List< TransactionsDetails> transactionsDetails = new List<TransactionsDetails>();
                decimal BigTotal = 0;
                var allLocationTransaction = await RepTransactionLocationTransaction.Find(x => x.IsActive == true && x.TransactionLocationId == id)
                    .AsQueryable().Include(x=> x.Transactions).ThenInclude(x=> x.TransactionsDetails).ToListAsync();
                foreach (var item in allLocationTransaction)
                {
                    item.IsActive = false;
                   
                    //Cambiar orden a completado
                    Guid StatusComple = Guid.Parse("85685D53-D6A6-4381-944B-995ED2667FBA");
                    var Invoice = await RepTransactionss.GetById(item.TransactionId);
                    Invoice.TransactionStatusId = StatusComple;
                    await RepTransactionss.Update(Invoice);
                    //Agrego las transacciones.
                    foreach (var itemDetails in item.Transactions.TransactionsDetails)
                    {
                       
                        var mapperIn = _mapper.Map<TransactionsDetailsDto>(itemDetails);
                        mapperIn.Id = Guid.Empty;
                        

                        transactionsDetails.Add(_mapper.Map<TransactionsDetails>(mapperIn));
                    }

                    BigTotal = BigTotal+ item.Transactions.GlobalTotal;



                    await RepTransactionLocationTransaction.SaveChangesAsync();
                }

                    var result = await RepTransactionss.SaveChangesAsync();
                    if (transactionsDetails.Count > 0)
                    {
                    transactions.GlobalTotal = BigTotal;
                        transactions.TransactionsDetails = transactionsDetails;
                        await TransactionService.TransactionProcess(transactions, FormId);
                    }

                return Ok(Result<TransactionsDto>.Success(_mapper.Map<TransactionsDto>(transactions), MessageCodes.AddedSuccessfully()));  

            }
            catch (Exception ex)
            {

                return Ok(Result<TransactionsDto>.Fail(ex.Message, "989", "", ""));
            }
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost([FromBody] PosDto data )
        {
            try
            {
                //Crear el contacto
                Contact _contact = new Contact();
                _contact.Name = data.Name;

                var resultContact = await RepContacts.InsertAsync(_contact);

                var DataSave = await RepContacts.SaveChangesAsync();



                Transactions transactions = new Transactions();
                transactions.ContactId = resultContact.Id;
                transactions.TransactionsType = data.TransactionType;
                transactions.GlobalTotal = data.Total;
                transactions.GlobalDiscount = 0;
                transactions.Date = DateTime.Now;

                List<TransactionsDetails> ListtransactionsDetails = new List<TransactionsDetails>();
                foreach (var item in data.TransactionsItems)
                {
                    TransactionsDetails transactionsDetails = new TransactionsDetails();
                    var concept = await RepConcept.GetById(item.Id);
                    decimal TotalSet = item.Count * concept.PriceSale.Value;
                    transactionsDetails.TransactionsId = transactions.Id;
                    transactionsDetails.ReferenceId = concept.Id;
                    transactionsDetails.Description = concept.Reference;
                    transactionsDetails.Amount = item.Count;
                    transactionsDetails.Price = concept.PriceSale.Value;
                    transactionsDetails.Discount = 0;
                    transactionsDetails.Tax = 0;
                    transactionsDetails.Total = TotalSet;
                     List<TransactionsDetailsElement> ListTransactionsDetailsElement = new List<TransactionsDetailsElement>();
                    foreach (var itemElement in item.ElementConcept)
                    {
                        if (itemElement.IsActive == false)
                        {

                            TransactionsDetailsElement transactionsDetailsElement = new TransactionsDetailsElement();
                            transactionsDetailsElement.TransactionsDetailsId = transactionsDetails.Id;
                            transactionsDetailsElement.ReferenceId = itemElement.ConceptId;
                            transactionsDetailsElement.Detaills = itemElement.Name;
                            ListTransactionsDetailsElement.Add(transactionsDetailsElement);
                        }

                    }
                    transactionsDetails.TransactionsDetailsElement = ListTransactionsDetailsElement;
                    ListtransactionsDetails.Add(transactionsDetails);
                }
                transactions.TransactionsDetails = ListtransactionsDetails;

                var result = await TransactionService.TransactionProcess(transactions, data.FormId);

                TransactionLocationTransaction _transactionLocationTransaction = new TransactionLocationTransaction();

                _transactionLocationTransaction.TransactionId = result.Id;

                _transactionLocationTransaction.TransactionLocationId = data.TransactionLocationId;

                var resulttran = await RepTransactionLocationTransaction.InsertAsync(_transactionLocationTransaction);

                var DataSavetran = await RepTransactionLocationTransaction.SaveChangesAsync();

                var mapperOut = _mapper.Map<TransactionsDto>(result);
                return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("CreateChangeTypes")]
        public async Task<IActionResult> CreateChangeTypes([FromBody]  int TransactionsType , Guid FormId, Guid TransactionsId)
        {
            try
            {
                var DataSave = await RepTransactionss.GetById(TransactionsId);

                DataSave.TransactionsType = TransactionsType;

                var result = await TransactionService.TransactionProcess(DataSave, FormId);
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
                var resultContact = await RepContacts.InsertAsync(mapperContact);
                var DataSave = await RepContacts.SaveChangesAsync();

                mapperIn.ContactId = resultContact.Id;
                var result = await TransactionService.TransactionProcess(mapperIn, data.FormId);
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
            var DataSave = await RepTransactionss.GetAll();
            var DataSaveDetails = await RepTransactionssDetails.GetAll();
            var DataFillter = DataSave.Where(x => x.IsActive == true).OrderByDescending(x => x.Date).ToList();
            foreach (var item in DataFillter)
            {
                item.TransactionsDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == item.Id).ToList();
            }
            return Ok(Result<IEnumerable<Transactions>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetBoxClose")]
        public async Task<IActionResult> GetBoxClose([FromQuery] DateTime startDate, DateTime endDate, int TransationType)
        {
            var query = await RepTransactionss.Find(x => x.IsActive == true
            & x.CreatedDate > startDate && x.CreatedDate < endDate
            & x.TransactionsType == TransationType).Include(x => x.PaymentMethods).ToListAsync();
            return Ok(Result<IEnumerable<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAllByContact")]
        public async Task<IActionResult> GetAllByContact(Guid ContactId)
        {

            var query = await RepTransactionss.Find(x => x.ContactId == ContactId && x.IsActive == true).
                 Include(x => x.Contact).
                     Include(s => s.TransactionStatus).
                Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept).FirstOrDefaultAsync();

            var data = query.TransactionsDetails.Where(x => x.IsActive = true).ToList();
            query.TransactionsDetails = data;

            return Ok(Result<Transactions>.Success(query, MessageCodes.AllSuccessfully()));

        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            try
            {
                var DataSave = await RepTransactionss.Find(x => x.Id == id)
                              .Include(x => x.Contact)
                              .Include(x => x.TransactionsDetails).ThenInclude(x => x.Concept)
                              .FirstOrDefaultAsync();
                var mapperOut = _mapper.Map<TransactionsContactDto>(DataSave);
                var mapperContact = _mapper.Map<TransactionsContactDto>(DataSave.Contact);
                if (mapperContact != null)
                {
                    mapperOut.Address = mapperContact.Address;

                    mapperOut.CellPhone = mapperContact.CellPhone;

                    mapperOut.DocumentNumber = mapperContact.DocumentNumber;

                    mapperOut.Name = mapperContact.Name;

                    mapperOut.Phone1 = mapperContact.Phone1;
                }
                return Ok(Result<TransactionsContactDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception)
            {

                return Ok(Result<TransactionsDto>.Fail("No tiene registros", MessageCodes.BabData()));
            }

        }
        [HttpGet("GetTicket")]
        public async Task<IActionResult> GetTicket([FromQuery] Guid id)
        {
            var Invoice = await RepTransactionss.Find(x => x.Id == id)
                .Include(x => x.Contact)
                .Include(x => x.PaymentTerms)
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync();

            var InvocieDetails = await RepTransactionssDetails.Find(x => x.IsActive == true
                  && x.TransactionsId == id).Include(x => x.Concept).ToListAsync();

            if (InvocieDetails.Count > 0)
                Invoice.TransactionsDetails = InvocieDetails;

            if (Invoice != null)
            {

                var CompanyFind = await RepCompanys.GetAll();

                var Company = CompanyFind.FirstOrDefault();

                var Ticket = new TicketDto();

                Ticket.CompanyId = Company.Id;

                Ticket.TaxId = Company.CompanyCode;

                Ticket.CompanyName = Company.CompanyName;

                Ticket.CompanyAdress = Company.Address;

                Ticket.CompanyPhones = Company.Phones;

                Ticket.InvoiceId = Invoice.Id;

                Ticket.InvoiceCode = Invoice.Code;

                Ticket.InvoiceDate = Invoice.Date;

                Ticket.InvoiceComentary = Invoice.Commentary;

                Ticket.InvoiceTotal = Invoice.GlobalTotal;

                if (Invoice.PaymentTermId != null)
                {

                    Ticket.InvoicePaymentTermId = Invoice.PaymentTermId;

                    Ticket.InvoicePaymentTerm = Invoice.PaymentTerms != null ? Invoice.PaymentTerms.Name : "Terminos no encontrado";

                }
                
                if (Invoice.PaymentMethodId != null)
                {
                    Ticket.InvoicePaymentMethodId = Invoice.PaymentMethodId;

                    Ticket.InvoicePaymentMethod = Invoice.PaymentMethods != null ? Invoice.PaymentMethods.Name : "Metodo no encontrado";
                }

                if (Invoice.ContactId != null)
                {
                    Ticket.InvoiceContactId = Invoice.ContactId;

                    Ticket.InvoiceContactName = Invoice.Contact.Name;

                    Ticket.InvoiceContactPhone = Invoice.Contact.Phone1 + " " + Invoice.Contact.Phone2 + " " + Invoice.Contact.CellPhone;

                    Ticket.InvoiceContactAdress = Invoice.Contact.Address;
                }
                if (Invoice.TransactionsDetails.Count > 0)
                {
                    var ListTicketDetallis = new List<TicketDetallisDto>();

                    foreach (var InvoiceDetallisRow in Invoice.TransactionsDetails)
                    {
                        var Concep = await RepConcept.GetById(InvoiceDetallisRow.ReferenceId);
                        if (Concep != null)
                        {
                            var TicketDetallis = new TicketDetallisDto();

                            TicketDetallis.ReferenceId = Concep.Id;

                            TicketDetallis.Total = InvoiceDetallisRow.Total;

                            TicketDetallis.Price = InvoiceDetallisRow.Price;

                            TicketDetallis.Amount = InvoiceDetallisRow.Amount;

                            TicketDetallis.Reference = InvoiceDetallisRow.Concept.Reference;

                            TicketDetallis.Description = InvoiceDetallisRow.Concept.Description;

                            ListTicketDetallis.Add(TicketDetallis);
                        }

                    }
                    if (ListTicketDetallis.Count > 0)
                    {
                        Ticket.TicketDetallisDtos = ListTicketDetallis;
                    }
                }
                return Ok(Result<TicketDto>.Success(Ticket, MessageCodes.AllSuccessfully()));

            }
            return Ok(Result<TicketDetallisDto>.Fail("No tiene registros", MessageCodes.BabData()));
        }
        [HttpGet("SetStatus")]
        public async Task<IActionResult> SetStatus([FromQuery] Guid TransactionId, Guid TransactionStatusId)
        {
            try
            {
                var Invoice = await RepTransactionss.GetById(TransactionId);

                Invoice.TransactionStatusId = TransactionStatusId;

                await RepTransactionss.Update(Invoice);
               var result  = await RepTransactionss.SaveChangesAsync();
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


            var query = await RepTransactionss.Find(x => x.Id == id).
                 Include(x => x.Contact).
                 Include(s => s.TransactionStatus).
                Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept).FirstOrDefaultAsync();


            return Ok(Result<Transactions>.Success(query, MessageCodes.AllSuccessfully()));
        }


        [HttpGet("GetAllByType")]
        public async Task<IActionResult> GetAllByType([FromQuery] int TransactionsTypeId)
        {
            var query = await RepTransactionss.Find(x => x.TransactionsType == TransactionsTypeId).Where(x => x.IsActive == true).
                 Include(x => x.Contact).
                 Include(x => x.PaymentMethods).
                 Include(x => x.PaymentTerms).
                 Include(s => s.TransactionStatus).
                 Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept)
                 .OrderByDescending(x => x.Date).ToListAsync();
            return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAllByTypeStatus")]
        public async Task<IActionResult> GetAllByTypeStatus([FromQuery] int TransactionsTypeId)
        {
            Guid Send = Guid.Parse("85685D53-D6A6-4381-944B-995ED2667FBA");
            Guid SendComplete = Guid.Parse("85685D53-D6A6-4381-944B-995ED1187FBA");
            var query = await RepTransactionss.Find(x => x.TransactionsType == 
            TransactionsTypeId && x.TransactionStatusId == Send || x.TransactionStatusId == SendComplete).Where(x => x.IsActive == true).
                 Include(x => x.Contact).
                 Include(x => x.PaymentMethods).
                 Include(x => x.PaymentTerms).
                 Include(s => s.TransactionStatus).
                 Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept)
                 .OrderBy(x => x.Date).ToListAsync();
            return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetAllByTypeStatusIsService")]
        public async Task<IActionResult> GetAllByTypeStatusIsService([FromQuery] int TransactionsTypeId, Guid TransactionStatusId)
        {
            var query = await RepTransactionss.Find(x => x.TransactionsType ==
            TransactionsTypeId && x.TransactionStatusId == TransactionStatusId).Where(x => x.IsActive == true  ).
                 Include(x => x.Contact).
                 Include(x => x.PaymentMethods).
                 Include(x => x.PaymentTerms).
                 Include(s => s.TransactionStatus).
                 Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept)
                 .OrderBy(x => x.Date).ToListAsync();
         
            List<Transactions> result = new List<Transactions>();
            foreach (var transaction in query)
            {
                bool isValide = false;
                foreach (var item in transaction.TransactionsDetails)
                {
                    if (item.Concept.IsServicie == true)
                    {
                        var IsLocation = await RepTransactionLocationTransaction.Find(x => x.TransactionId == transaction.Id).FirstOrDefaultAsync();
                        if (IsLocation.IsActive)
                        {
                         isValide = true;
                        }
                    }
                }
                if (isValide)
                {

                result.Add(transaction);
                }

            }

            return Ok(Result<List<Transactions>>.Success(result, MessageCodes.AllSuccessfully()));
        }


        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionsDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter, int TransactionsTypeId)
        {
            var firtFilter = RepTransactionss.Find(x => x.IsActive == true && x.TransactionsType == TransactionsTypeId).Include(x => x.Contact).
                 Include(x => x.PaymentMethods).
                 Include(x => x.PaymentTerms).
                 Include(s => s.TransactionStatus).
                 Include(s => s.Contact).
                 Include(x => x.TransactionsDetails).Where(x => x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
             || x.Reference.ToLower().Contains(filter.Search.Trim().ToLower())
             || x.Contact.Name.ToLower().Contains(filter.Search.Trim().ToLower())
            ).OrderByDescending(x => x.CreatedDate).ToList();

            int totalRecords = firtFilter.Count();
            var DataMaperOut = _mapper.Map<List<TransactionsDto>>(firtFilter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionsDto>>.Success(List);
            return Ok(Result);

        }

        [HttpGet("GetAllByTypeDisableRow")]
        public async Task<IActionResult> GetAllByTypeDisableRow([FromQuery] int TransactionsTypeId)
        {



            var query = await RepTransactionss.Find(x => x.TransactionsType == TransactionsTypeId).Where(x => x.IsActive == false).
                 Include(x => x.Contact).
                 Include(x => x.PaymentMethods).
                 Include(x => x.PaymentTerms).
                     Include(s => s.TransactionStatus).
                Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept).ToListAsync();


            return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
        }



        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var re = Request;
            var headers = re.Headers;
            string Token = Request.Headers["Authorization"];
            var Data = await RepTransactionss.GetById(id);

            Data.IsActive = false;

            await RepTransactionss.Update(Data);

            var save = await RepTransactionss.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionsDto>(Data);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpDelete("TransactionssDetailsDelete/{id}")]

        public async Task<IActionResult> TransactionssDetailsDelete(Guid id)
        {
            var re = Request;
            var headers = re.Headers;
            string Token = Request.Headers["Authorization"];
            var Data = await RepTransactionssDetails.GetById(id);

            Data.IsActive = false;

            await RepTransactionssDetails.Update(Data);

            var save = await RepTransactionssDetails.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionsDetailsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionsDetailsDto>(Data);

            return Ok(Result<TransactionsDetailsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionsDto _UpdateDto)
        {
            var mapperIn = _mapper.Map<Transactions>(_UpdateDto);
            var result = await TransactionService.TransactionProcess(mapperIn, _UpdateDto.FormId);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

    }



}
