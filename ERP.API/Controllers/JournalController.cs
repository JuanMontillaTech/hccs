using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants; 
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using System.Net;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IGenericRepository<Journal> _repJournals;
        private   readonly IGenericRepository<JournaDetails> _repJournalsDetails;
        private readonly IGenericRepository<ConfigurationReport> _repConfigurationReport;

        

        private readonly INumerationService _numerationService;
        private readonly IGenericRepository<LedgerAccount> _repLedgerAccounts;


        private readonly IMapper _mapper;

        public JournalController(IGenericRepository<Journal> repJournals,
            IGenericRepository<JournaDetails> repJournalsDetails,
            IGenericRepository<LedgerAccount> repLedgerAccounts, 
            IGenericRepository<ConfigurationReport> repConfigurationReport,
            IMapper mapper,
            INumerationService numerationService)
        {
            this._numerationService = numerationService;
            _repJournals = repJournals;
            _repJournalsDetails = repJournalsDetails;
            _repLedgerAccounts = repLedgerAccounts;
            _repConfigurationReport = repConfigurationReport;
           
            _mapper = mapper;
        }

        #region NewAccount

        private async Task<List<ConfigurationReport>> GetAccountByCode(string Code)
        {
            var query = await _repConfigurationReport.Find(x => x.Code == Code)
                .Include(x => x.LedgerAccount).OrderBy(x => x.Index).ToListAsync();
            return query;
        }


        private async Task<LedgerAccountwihtBalance> GetAllDataById(Guid LedgerAccountId)
        {
            var query = await _repJournalsDetails.Find(x => x.LedgerAccountId == LedgerAccountId && x.IsActive == true)
                .Include(x => x.LedgerAccount).ToListAsync();
            var row = query.FirstOrDefault();
            if (query.Count == 0)
                return null;


            LedgerAccountwihtBalance journaDetails = new LedgerAccountwihtBalance();
            journaDetails.Code = row.LedgerAccount.Code;
            journaDetails.Name = row.LedgerAccount.Name;
            journaDetails.LedgerAccountId = row.LedgerAccountId;
            journaDetails.Credit = query.Where(x => x.IsActive == true).Sum(x => x.Credit);
            journaDetails.Creditor = journaDetails.Debit > journaDetails.Credit
                ? journaDetails.Debit - journaDetails.Credit
                : 0;
            journaDetails.Debitor = journaDetails.Credit > journaDetails.Debit
                ? journaDetails.Credit - journaDetails.Debit
                : 0;
            journaDetails.Debit = query.Where(x => x.IsActive == true).Sum(x => x.Debit);
            journaDetails.Balance = journaDetails.Credit > journaDetails.Debit
                ? journaDetails.Credit - journaDetails.Debit
                : journaDetails.Debit - journaDetails.Credit;

            return journaDetails;
        }

 

        private async Task<LedgerAccountwihtBalance> GetAllDataByLedgerAcountWithMonth(Guid LedgerAccountId, int Month)
        {
            var transaccionList = await _repJournals.Find(x => x.IsActive == true)
                .Include(x => x.JournaDetails)
                .Where(x => x.Date.Month == Month && x.IsActive == true).ToListAsync();

            var account = await _repLedgerAccounts.GetById(LedgerAccountId);

            LedgerAccountwihtBalance journaDetails = new LedgerAccountwihtBalance();

            journaDetails.Code = account.Code;
            journaDetails.Name = account.Name;
            journaDetails.LedgerAccountId = LedgerAccountId;

            List<Journal> journalsList = new();

            foreach (var RowTrans in transaccionList)
            {
                var detailsList = RowTrans.JournaDetails.Where(x => x.LedgerAccountId == LedgerAccountId
                                                                    && x.IsActive == true && x.JournalId == RowTrans.Id)
                    .ToList();
                journaDetails.Credit = detailsList.Where(x => x.IsActive == true).Sum(x => x.Credit);
                journaDetails.Debit = detailsList.Where(x => x.IsActive == true).Sum(x => x.Debit);
            }


            journaDetails.Creditor = journaDetails.Debit > journaDetails.Credit
                ? journaDetails.Debit - journaDetails.Credit
                : 0;
            journaDetails.Debitor = journaDetails.Credit > journaDetails.Debit
                ? journaDetails.Credit - journaDetails.Debit
                : 0;

            journaDetails.Balance = journaDetails.Credit > journaDetails.Debit
                ? journaDetails.Credit - journaDetails.Debit
                : journaDetails.Debit - journaDetails.Credit;

            return journaDetails;
        }

        [HttpGet("GetAllLedgerAccountByCode")]
        public async Task<IActionResult> GetAllLedgerAccountByCode([FromQuery] string Code)
        {
            var calalogo = await GetAccountByCode(Code);
            List<LedgerAccountwihtBalance> ledgerAccountwihtBalances = new List<LedgerAccountwihtBalance>();
            foreach (var item in calalogo)
            {
                var generalLager = await GetAllDataById((Guid)item.Parameter);
                if (generalLager != null)
                {
                    generalLager.Origen = item.Criterion.Trim().Length > 0 ? int.Parse(item.Criterion) : 0;
                    ledgerAccountwihtBalances.Add(generalLager);
                }
            }

            return
                Ok(Result<List<LedgerAccountwihtBalance>>.Success(ledgerAccountwihtBalances,
                    MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetAllLedgerAccountByCodeMonth")]
        public async Task<IActionResult> GetAllLedgerAccountByCodeMonth([FromQuery] string Code, int Month)
        {
            var calalogo = await GetAccountByCode(Code);
            List<LedgerAccountwihtBalance> ledgerAccountwihtBalances = new List<LedgerAccountwihtBalance>();
            foreach (var item in calalogo)
            {
                var GeneralLager = await GetAllDataByLedgerAcountWithMonth((Guid)item.Parameter, Month);
                if (GeneralLager != null)
                {
                    GeneralLager.Origen = item.Criterion.Trim().Length > 0 ? int.Parse(item.Criterion) : 0;
                    ledgerAccountwihtBalances.Add(GeneralLager);
                }
            }

            return Ok(Result<List<LedgerAccountwihtBalance>>.Success(ledgerAccountwihtBalances,
                MessageCodes.AllSuccessfully()));
        }

        #endregion

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] JournalDto data)
        {
            try
            {
                var mapper = _mapper.Map<Journal>(data); 
                mapper.JournaDetails =  mapper.JournaDetails.Where(x => x.IsActive == true).ToList();
                string nextNumber = await _numerationService.GetNextDocumentAsync((Guid)mapper.TypeRegisterId);
                mapper.Code = nextNumber;
                var result = await _repJournals.InsertAsync(mapper);
                var DataSave = await _repJournals.SaveChangesAsync();

                await _numerationService.SaveNextNumber((Guid)mapper.TypeRegisterId);

                if (DataSave != 1)
                    return Ok(Result<JournalDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<JournalDto>(result);


                return Ok(Result<JournalDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
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
            var DataSave = await RepJournals.GetAll();
            var DataSaveDetails = await RepJournalsDetails.Find(x => x.IsActive == true).
                                    Include(x => x.LedgerAccount).ToListAsync();
            var DataFillter = DataSave.Where(x => x.IsActive == true).ToList().OrderByDescending(x=>x.Date);
            foreach (var item in DataFillter)
            {
                item.JournaDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.JournalId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Journal>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<Journal>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var Filter = _repJournals.Find(x => x.IsActive == true)
                .Where(x => x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
                            || (x.Reference.ToLower().Contains(filter.Search.Trim().ToLower()))
                ).ToList().OrderByDescending(x => x.CreatedDate);


            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<Journal>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<Journal>>.Success(List);
            return Ok(Result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await _repJournals.Find(x => x.IsActive == true && x.Id == id)
                .Include(x => x.JournaDetails.Where(x => x.IsActive == true))
                .FirstOrDefaultAsync();

            var mapperOut = _mapper.Map<JournalDto>(DataSave);

            return Ok(Result<JournalDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await _repJournals.GetById(id);

            Data.IsActive = false;

            await _repJournals.Update(Data);

            var save = await _repJournals.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<JournalIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<JournalDto>(Data);

            return Ok(Result<JournalDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] JournalDto _UpdateDto)
        {
            var updateData = await _repJournals.GetById(_UpdateDto.Id);
            updateData.Code = _UpdateDto.Code;
            updateData.Reference = _UpdateDto.Reference;
            updateData.Commentary = _UpdateDto.Commentary;
            updateData.Date = _UpdateDto.Date;
            await _repJournals.Update(updateData);
            await _repJournals.SaveChangesAsync();
             
            foreach (var intRow in _UpdateDto.JournaDetails)
            {
                if (intRow.Id != null)
                {
                    var rows = await _repJournalsDetails.GetById(intRow.Id);
                    if (rows != null )
                    {
                        if (intRow.ContactId != null)
                            rows.ContactId = intRow.ContactId;
                        rows.JournalId = rows.JournalId;
                        rows.Commentary = intRow.Commentary;
                        rows.LedgerAccountId = intRow.LedgerAccountId;
                        rows.Debit = intRow.Debit;
                        rows.Credit = intRow.Credit;
                        rows.IsActive =  intRow.IsActive;
                    }
                }
                else
                {
                    if (!intRow.IsActive) continue;
                    var rows = new JournaDetails();
                    if (intRow.ContactId != null)
                        rows.ContactId = intRow.ContactId;

                    rows.JournalId = updateData.Id;
                    rows.Commentary = intRow.Commentary;
                    rows.LedgerAccountId = intRow.LedgerAccountId;
                    rows.Debit = intRow.Debit;
                    rows.Credit = intRow.Credit;
                    rows.IsActive = intRow.IsActive;

                    await _repJournalsDetails.InsertAsync(rows);
                }
            }

            var data = await _repJournalsDetails.SaveChangesAsync();


            return Ok(Result<Journal>.Success(updateData, MessageCodes.UpdatedSuccessfully()));
        }
    }
}

