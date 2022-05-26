
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

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly IGenericRepository<Transactions> RepTransactionss;
        public readonly IGenericRepository<TransactionsDetails> RepTransactionssDetails;
        public readonly INumerationService numerationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IGenericRepository<Journal> RepJournals;
        public readonly IGenericRepository<JournaDetails> RepJournalsDetails;
        public readonly IGenericRepository<Concept> RepConcept;
        private readonly IMapper _mapper;
        public TransactionController(IGenericRepository<Transactions> repTransactionss,
        IGenericRepository<TransactionsDetails> repTransactionssDetails, IMapper mapper, IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails,
        IGenericRepository<Concept> _RepConcept,
        INumerationService numerationService, IHttpContextAccessor httpContextAccessor)
        {
            RepJournals = repJournals;
            RepConcept = _RepConcept;
            RepJournalsDetails = repJournalsDetails;
            this.numerationService = numerationService;
            RepTransactionss = repTransactionss;
            RepTransactionssDetails = repTransactionssDetails;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDto data)
        {
            try
            {
                bool IsForJournal = false;
               

                string Token = Request.Headers["Authorization"];
                var mapper = _mapper.Map<Transactions>(data);
                Guid NumberId = Guid.Empty;
                switch (data.TransactionsType)
                {
                    case 1:
                        NumberId = Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb");
                        //Recibo de concepto
                        IsForJournal = true;
                        break;
                    case 2:
                        NumberId = Guid.Parse("974E09C8-1D92-4587-A195-0D1345B27A65");
                        //Comprobante de compras (11)
                        IsForJournal = true;
                        break;
                    case 3:
                        NumberId = Guid.Parse("A1FC3EFA-11DD-4340-8CE8-DF005EC1A8C2");
                        //Numeración cotizaciones
                        break;
                    case 4:
                        //Numeración manual notas débito
                        NumberId = Guid.Parse("F39DBB77-5433-4D0E-BF50-8087CD57FB5A");
                        break;
                    case 5:
                        NumberId = Guid.Parse("A1FC3EFA-11DD-4340-8CE8-DF005EC1A8C2");
                        //Numeración cotizaciones
                        break;
                    case 6:
                        NumberId = Guid.Parse("541084E9-1007-4618-87DF-0681E69A0414");
                        //Numeración conduces
                        break;
                    case 7:
                        NumberId = Guid.Parse("AD45DA9D-5390-41D4-8451-D1A4A3D227C2");
                        //Numeración órdenes de compra
                        break;               
                       
                    default:
                        NumberId = Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb");
                        //Recibo de concepto
                        break;
                }
                string nextNumber = await numerationService.GetNextDocumentAsync(NumberId);
                mapper.Code = nextNumber;
                var result = await RepTransactionss.Insert(mapper);
                var DataSave = await RepTransactionss.SaveChangesAsync();
                await numerationService.SaveNextNumber(NumberId);

                //Contabilidad
                if (IsForJournal)
                {
                    foreach (var TranSrow in data.TransactionsDetails)
                    {
                        var _Concepto = await RepConcept.GetById(TranSrow.ReferenceId);

                        if (_Concepto != null)
                        {                                                        
                            Journal journal = new Journal();
                            journal.Date = mapper.Date;
                            journal.Reference =  mapper.Code;
                            journal.TypeRegisterId = NumberId;
                            List<JournaDetails> LjournaDetails = new();
                            JournaDetails journaDetails = new JournaDetails();
                            if (data.TransactionsType == 1)
                            {
                                journaDetails.Credit = data.GlobalTotal;
                                journaDetails.Debit = 0;
                                journaDetails.LedgerAccountId = _Concepto.CreditLedgerAccountId;
                            }
                            else
                            {
                                journaDetails.Credit = 0;
                                journaDetails.Debit = data.GlobalTotal;
                                journaDetails.LedgerAccountId = _Concepto.DebitLedgerAccountId;
                            }
                            LjournaDetails.Add(journaDetails);
                            journal.JournaDetails = LjournaDetails;
                            string nextNumberc = await numerationService.GetNextDocumentAsync((Guid)journal.TypeRegisterId);
                            journal.Code = nextNumberc;
                            var resultw = await RepJournals.Insert(journal);
                            var DataSavew = await RepJournals.SaveChangesAsync();
                            await numerationService.SaveNextNumber((Guid)journal.TypeRegisterId);
                        }
                    }

                }
                              
                //Fin contabilidad

                if (DataSave != 1)
                    return Ok(Result<TransactionsDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<TransactionsDto>(result);


                return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (System.Exception ex)
            {
                string mg = ex.Message;
                throw;
            }


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           


            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user =  _httpContextAccessor.HttpContext.Items["Email"];
            var DataSave = await RepTransactionss.GetAll();
            var DataSaveDetails = await RepTransactionssDetails.GetAll();
            var DataFillter = DataSave.Where(x => x.IsActive == true).ToList();
            foreach (var item in DataFillter)
            {
                item.TransactionsDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Transactions>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAllByContact")]
        public async Task<IActionResult> GetAllByContact(Guid ContactId)
        {

            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _httpContextAccessor.HttpContext.Items["Email"];
            var DataSave = await RepTransactionss.GetAll();
            var DataSaveDetails = await RepTransactionssDetails.GetAll();
            var DataFillter = DataSave.Where(x => x.IsActive == true && x.ContactId == ContactId).ToList();
            foreach (var item in DataFillter)
            {
                item.TransactionsDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Transactions>>.Success(DataFillter, MessageCodes.AllSuccessfully()));


        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTransactionss.GetById(id);

            
            var DataSaveDetails = await RepTransactionssDetails.GetAll(); 
            
               var transationDetalli = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.TransactionsId == id).ToList();
            DataSave.TransactionsDetails = transationDetalli;
            var mapperOut = _mapper.Map<TransactionsDto>(DataSave);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
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
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionsDto _UpdateDto)
        {
            bool IsForJournal = false;
            var UpdateData = await RepTransactionss.GetById(_UpdateDto.Id);
            UpdateData.ContactId = _UpdateDto.ContactId;
            UpdateData.Code = _UpdateDto.Code;
            UpdateData.Date = _UpdateDto.Date;
            UpdateData.Reference = _UpdateDto.Reference;
            UpdateData.PaymentMethodId = _UpdateDto.PaymentMethodId;
            UpdateData.GlobalDiscount = _UpdateDto.GlobalDiscount;
            UpdateData.GlobalTotal = _UpdateDto.GlobalTotal;            
            UpdateData.Commentary = _UpdateDto.Commentary;
            var result = await RepTransactionss.Update(UpdateData);
            var DataSave = await RepTransactionss.SaveChangesAsync();
            var DataAll = await RepTransactionssDetails.GetAll();
            foreach (var item in DataAll.Where(x => x.TransactionsId == _UpdateDto.Id).ToList())
                item.IsActive = false;


            foreach (var intRow in _UpdateDto.TransactionsDetails)
            {
                if (intRow.Id != null)
                {

                    var rows = await RepTransactionssDetails.GetById(intRow.Id);
                    if (rows != null)
                    {
                        if (intRow.ReferenceId != null)
                          rows.ReferenceId = intRow.ReferenceId;
                        rows.TransactionsId = rows.TransactionsId;
                        rows.Description = intRow.Description;
                        rows.Amount = intRow.Amount;
                        rows.Price = intRow.Price;
                        rows.Discount = intRow.Discount;
                        rows.Total = intRow.Total;
                        rows.Tax = intRow.Tax;
                        rows.Commentary = intRow.Commentary;
                        rows.IsActive = true;
                    }

                }
                else
                {

                    var rows = new TransactionsDetails();
                    if (intRow.ReferenceId != null)
                        rows.ReferenceId = intRow.ReferenceId;
                    rows.TransactionsId = rows.TransactionsId;
                    rows.Description = intRow.Description;
                    rows.Amount = intRow.Amount;
                    rows.Price = intRow.Price;
                    rows.Discount = intRow.Discount;
                    rows.Total = intRow.Total;
                    rows.Tax = intRow.Tax;
                    rows.Commentary = intRow.Commentary;
                    rows.IsActive = true;

                    var insert = await RepTransactionssDetails.Insert(rows);

                }


            }
            switch (_UpdateDto.TransactionsType)
            {
                case 1:
                    //NumberId = Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb");
                    //Recibo de concepto
                    IsForJournal = true;
                    break;
                case 2:
                  //  NumberId = Guid.Parse("974E09C8-1D92-4587-A195-0D1345B27A65");
                    //Comprobante de compras (11)
                    IsForJournal = true;
                    break;
                case 3:
                   // NumberId = Guid.Parse("A1FC3EFA-11DD-4340-8CE8-DF005EC1A8C2");
                    //Numeración cotizaciones
                    break;
                case 4:
                    //Numeración manual notas débito
                   // NumberId = Guid.Parse("F39DBB77-5433-4D0E-BF50-8087CD57FB5A");
                    break;
                case 5:
                   // NumberId = Guid.Parse("A1FC3EFA-11DD-4340-8CE8-DF005EC1A8C2");
                    //Numeración cotizaciones
                    break;
                case 6:
                   // NumberId = Guid.Parse("541084E9-1007-4618-87DF-0681E69A0414");
                    //Numeración conduces
                    break;
                case 7:
                   // NumberId = Guid.Parse("AD45DA9D-5390-41D4-8451-D1A4A3D227C2");
                    //Numeración órdenes de compra
                    break;

                default:
                   // NumberId = Guid.Parse("5dc90864-a835-4582-917c-53e5209feaeb");
                    //Recibo de concepto
                    break;
            }


            var data = await RepTransactionssDetails.SaveChangesAsync();
            //Contabilidad
            if (IsForJournal)
            {
                foreach (var TranSrow in UpdateData.TransactionsDetails)
                {
                    var _Concepto = await RepConcept.GetById(TranSrow.ReferenceId);
                    var foundJun = await RepJournals.GetAll();
                    foreach (var item in foundJun.Where(x=> x.TypeRegisterId == UpdateData.Id).ToList())                    
                        item.IsActive = false;
              
                    if (_Concepto != null)
                    {
                        Journal journal = new Journal();
                        journal.Date = UpdateData.Date;
                        journal.Reference = UpdateData.Code; 
                        List<JournaDetails> LjournaDetails = new();
                        JournaDetails journaDetails = new JournaDetails();
                        if (UpdateData.TransactionsType == 1)
                        {
                            journaDetails.Credit = UpdateData.GlobalTotal;
                            journaDetails.Debit = 0;
                            journaDetails.LedgerAccountId = _Concepto.CreditLedgerAccountId;
                        }
                        else
                        {
                            journaDetails.Credit = 0;
                            journaDetails.Debit = UpdateData.GlobalTotal;
                            journaDetails.LedgerAccountId = _Concepto.DebitLedgerAccountId;
                        }
                        LjournaDetails.Add(journaDetails);
                        journal.JournaDetails = LjournaDetails;
                        string nextNumberc = await numerationService.GetNextDocumentAsync((Guid)journal.TypeRegisterId);
                        journal.Code = nextNumberc;
                        var resultw = await RepJournals.Insert(journal);

                        var DataSavew = await RepJournals.SaveChangesAsync();
                        await numerationService.SaveNextNumber((Guid)journal.TypeRegisterId);
                    }
                }

            }

            //Fin contabilidad


            return Ok(Result<Transactions>.Success(UpdateData, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
