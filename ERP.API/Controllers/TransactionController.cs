
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

        private readonly ITransactionService TransactionService;

        public TransactionController(IGenericRepository<Transactions> repTransactionss,
        IGenericRepository<TransactionsDetails> repTransactionssDetails, IMapper mapper, IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails,
        IGenericRepository<Concept> _RepConcept,
        INumerationService numerationService, IHttpContextAccessor httpContextAccessor, ITransactionService transactionService)
        {
            RepJournals = repJournals;
            RepConcept = _RepConcept;
            RepJournalsDetails = repJournalsDetails;
            this.numerationService = numerationService;
            RepTransactionss = repTransactionss;
            RepTransactionssDetails = repTransactionssDetails;
            _httpContextAccessor = httpContextAccessor;
            TransactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDto data)
        {
            
            var mapperIn = _mapper.Map<Transactions>(data);
            var  result = await TransactionService.TransactionProcess(mapperIn, data.FormId);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
          


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
             
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
            var DataSave = await RepTransactionss.GetById(id);


            var DataSaveDetails = await RepTransactionssDetails.GetAll();

            var transationDetalli = DataSaveDetails.AsQueryable()
                  .Where(x => x.IsActive == true && x.TransactionsId == id).ToList();
            DataSave.TransactionsDetails = transationDetalli;
            var mapperOut = _mapper.Map<TransactionsDto>(DataSave);

            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
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

          

            var query = await RepTransactionss.Find(x => x.TransactionsType == TransactionsTypeId).
                 Include(x => x.Contact).
                 Include(x=> x.PaymentMethods).
                 Include(x=> x.PaymentTerms).
                     Include(s => s.TransactionStatus).
                Include(x => x.TransactionsDetails).ThenInclude(X => X.Concept).ToListAsync();


            return Ok(Result<List<Transactions>>.Success(query, MessageCodes.AllSuccessfully()));
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
            var mapperIn = _mapper.Map<Transactions>(_UpdateDto);
            var result = await TransactionService.TransactionProcess(mapperIn, _UpdateDto.FormId);
            var mapperOut = _mapper.Map<TransactionsDto>(result);
            return Ok(Result<TransactionsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

    }



}
