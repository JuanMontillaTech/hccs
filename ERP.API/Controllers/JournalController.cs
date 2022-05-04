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

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class JournalController : ControllerBase
    {
        public readonly IGenericRepository<Journal> RepJournals;
        public readonly IGenericRepository<JournaDetails> RepJournalsDetails;
        public readonly IGenericRepository<ConfigurationReport> RepConfigurationReport;

        public readonly IGenericRepository<Company> RepCompany;

        public readonly INumerationService numerationService;
        public readonly IGenericRepository<LedgerAccount> RepLedgerAccounts;


        private readonly IMapper _mapper;
        public JournalController(IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails,
        IGenericRepository<LedgerAccount> _RepLedgerAccounts,
        IGenericRepository<Company> _RepCompany,
         IGenericRepository<ConfigurationReport> _RepConfigurationReport,
        IMapper mapper,
        INumerationService numerationService)
        {
            this.numerationService = numerationService;
            RepJournals = repJournals;
            RepJournalsDetails = repJournalsDetails;
            RepLedgerAccounts = _RepLedgerAccounts;
            RepConfigurationReport = _RepConfigurationReport;
            RepCompany = _RepCompany;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] JournalDto data)
        {
            try
            {
                var mapper = _mapper.Map<Journal>(data);
                mapper.TypeRegisterId = Guid.Parse("DC4678AF-AF3C-4E90-9356-379D336EB03C");
                string nextNumber = await numerationService.GetNextDocumentAsync(Guid.Parse("5E17B36A-FBBE-4C73-93AC-B112EE3FF08A"));
                mapper.Code = nextNumber;
                var result = await RepJournals.Insert(mapper);
                var DataSave = await RepJournals.SaveChangesAsync();
                await numerationService.SaveNextNumber(Guid.Parse("5E17B36A-FBBE-4C73-93AC-B112EE3FF08A"));

                if (DataSave != 1)
                    return Ok(Result<JournalIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<JournalIdDto>(result);


                return Ok(Result<JournalIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
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
            var DataSaveDetails = await RepJournalsDetails.GetAll();
            var DataFillter = DataSave.Where(x => x.IsActive == true).ToList();
            foreach (var item in DataFillter)
            {
                item.JournaDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.JournalId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Journal>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
        }


        [HttpGet("MajorGeneral")]
        public async Task<IActionResult> MajorGeneral()
        {
            List<MajorGeneralDto> mjgLit = await GetGeneralMajor();

            return Ok(Result<List<MajorGeneralDto>>.Success(mjgLit, MessageCodes.AllSuccessfully()));


        }


        [HttpGet("Semester")]
        public async Task<IActionResult> Semester(string Criterion, string Code, int MonthStart, int MonthEnd)
        {
            try
            {
                var companyData = await RepCompany.GetAll(); var company = companyData.FirstOrDefault();
                AccountsBalanceDto AllCounBalance = new();
                AllCounBalance.Company = _mapper.Map<CompanyDto>(company);
                AllCounBalance.Criterion = Criterion;
                AllCounBalance.Code = Code;
                var reportConfigures = await RepConfigurationReport.GetAll();
                List<AccountMonthGroupDto> ListAccountMonth = new();
                foreach (var Icome in reportConfigures.Where(x => x.Code == AllCounBalance.Code && x.Criterion == AllCounBalance.Criterion).ToList())
                {
                    var AccoundBalanceMoth = await GetAccountByMonthBalance(Guid.Parse(Icome.Parameter.ToString()), MonthStart, MonthEnd);
                    ListAccountMonth.Add(AccoundBalanceMoth);
                }
                AllCounBalance.AccountMonthGroup = ListAccountMonth;

                return Ok(Result<AccountsBalanceDto>.Success(AllCounBalance, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {

                return Ok(Result<string>.Fail(ex.Message, ex.Message));
            }

        }

     




        private async Task<AccountMonthGroupDto> GetAccountByMonthBalance(Guid AccountId, int MonthStart, int MonthEnd)
        {
            var Account = await RepLedgerAccounts.GetById(AccountId);
            AccountMonthGroupDto mdtlist = new AccountMonthGroupDto();
            mdtlist.Id = AccountId;
            mdtlist.Name = Account.Name;
            mdtlist.Code = Account.Code;
            MonthEnd = MonthEnd + 1;
            List<AccountMonthBalanceDto> AccountList = new List<AccountMonthBalanceDto>();
            for (int i = MonthStart; i < MonthEnd; i++)
            {
                AccountMonthBalanceDto mg = new AccountMonthBalanceDto();
                mg.Id = Account.Id;
                mg.MonthNumber = i;
                switch (mg.MonthNumber)
                {
                    case 1: mg.Month = "Enero"; break;
                    case 2: mg.Month = "Febrero"; break;
                    case 3: mg.Month = "Marzo"; break;
                    case 4: mg.Month = "Abril"; break;
                    case 5: mg.Month = "Mayo"; break;
                    case 6: mg.Month = "Junio"; break;
                    case 7: mg.Month = "Julio"; break;
                    case 8: mg.Month = "Agosto"; break;
                    case 9: mg.Month = "Septiembre"; break;
                    case 10: mg.Month = "Octubre"; break;
                    case 11: mg.Month = "Noviembre"; break;
                    case 12: mg.Month = "Diciembre"; break;
                    default: mg.Month = "Diciembre"; break;
                }

                var data = await GetBalanceAccountOutDetalle(mg.Id, MonthStart);
                mg.MajorGeneralDto = data;
                AccountList.Add(mg);

            }
            mdtlist.AccountMonthBalance = AccountList;
            return mdtlist;

        }


        private async Task<MajorGeneralDto> GetBalanceAccountOutDetalle(Guid AccountId, int Month)
        {

            var DataSaveDetails = await RepJournalsDetails.GetAll();
            MajorGeneralDto mg = new MajorGeneralDto();

            decimal Debit = 0;
            decimal Credit = 0;
            List<MajorGeneralDetallsDto> mdtlist = new List<MajorGeneralDetallsDto>();
            foreach (var item in DataSaveDetails.AsQueryable()
                 .Where(x => x.IsActive == true && x.LedgerAccountId == AccountId).ToList())
            {
                var JournalsRow = await RepJournals.GetById(item.JournalId);
                if (JournalsRow != null)
                {


                    if (JournalsRow.IsActive)
                    {
                        if (Month > 0)
                        {

                            int month = JournalsRow.Date.Month;
                            if (month == Month)
                            {

                                var newDetallis = new MajorGeneralDetallsDto();
                                newDetallis.AccountId = item.LedgerAccountId;
                                newDetallis.Debit = item.Debit;
                                newDetallis.Credit = item.Credit;
                                newDetallis.Code = JournalsRow.Code;
                                newDetallis.Date = JournalsRow.Date;
                                mdtlist.Add(newDetallis);
                                Debit += item.Debit;
                                Credit += item.Credit;
                            }
                        }
                        else
                        {
                            var newDetallis = new MajorGeneralDetallsDto();
                            newDetallis.AccountId = item.LedgerAccountId;
                            newDetallis.Debit = item.Debit;
                            newDetallis.Credit = item.Credit;
                            newDetallis.Code = JournalsRow.Code;
                            newDetallis.Date = JournalsRow.Date;
                            mdtlist.Add(newDetallis);
                            Debit += item.Debit;
                            Credit += item.Credit;
                        }
                    }
                }

            }
            mg.MajorGeneralDetalls = mdtlist;
            mg.TotalDebit = Debit;
            mg.TotalCredit = Credit;
            if (Debit > Credit)
            {
                mg.Debtor = Debit - Credit;
            }
            else
            {
                mg.Debtor = 0;
            }
            if (Credit > Debit)
            {
                mg.Creditor = Credit - Debit;
            }
            else
            {
                mg.Creditor = 0;
            }

            return mg;
        }



        private async Task<List<MajorGeneralDto>> GetGeneralMajor()
        {
            var RepAccountAll = await RepLedgerAccounts.GetAll();
            var ParentAccountAll = RepAccountAll.Where(x => x.IsActive == true && x.Belongs == null).ToList();

            foreach (var item in ParentAccountAll)
            {
                LedgerAccountWithParent ledgerAccountWithParent = new LedgerAccountWithParent();
                ledgerAccountWithParent.Id = item.Id;
                ledgerAccountWithParent.IsActive = item.IsActive;
                ledgerAccountWithParent.Name = item.Name;
                ledgerAccountWithParent.Code = item.Code;
                ledgerAccountWithParent.LocationStatusResult = item.LocationStatusResult;
                ledgerAccountWithParent.Nature = item.Nature;
                ledgerAccountWithParent.Belongs = item.Belongs;

                var Chilfound = RepAccountAll.Where(x => x.IsActive == true && x.Belongs == item.Id).ToList();
                foreach (var Childrends in Chilfound)
                {
                    LedgerAccountWithParent sons = new LedgerAccountWithParent();
                    sons.Id = Childrends.Id;
                    sons.IsActive = Childrends.IsActive;
                    sons.Name = Childrends.Name;
                    sons.Code = Childrends.Code;
                    sons.LocationStatusResult = Childrends.LocationStatusResult;
                    sons.Nature = Childrends.Nature;
                    sons.Belongs = Childrends.Belongs;
                }


            }

            List<MajorGeneralDto> mjgLit = new List<MajorGeneralDto>();
            foreach (var Account in RepAccountAll.Where(x => x.IsActive == true).ToList())
            {
                var DataSaveDetails = await RepJournalsDetails.GetAll();
                MajorGeneralDto mg = new MajorGeneralDto();
                mg.Id = Account.Id;
                mg.Name = Account.Name;
                mg.AccountNumber = Account.Code;
                decimal Debit = 0;
                decimal Credit = 0;
                List<MajorGeneralDetallsDto> mdtlist = new List<MajorGeneralDetallsDto>();
                foreach (var item in DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.LedgerAccountId == Account.Id).ToList())
                {
                    var JournalsRow = await RepJournals.GetById(item.JournalId);
                    if (JournalsRow != null)
                    {
                        if (JournalsRow.IsActive)
                        {
                            var newDetallis = new MajorGeneralDetallsDto();
                            newDetallis.AccountId = item.LedgerAccountId;
                            newDetallis.Debit = item.Debit;
                            newDetallis.Credit = item.Credit;
                            newDetallis.Code = JournalsRow.Code;
                            newDetallis.Date = JournalsRow.Date;
                            mdtlist.Add(newDetallis);
                            Debit += item.Debit;
                            Credit += item.Credit;
                        }
                    }

                }
                mg.MajorGeneralDetalls = mdtlist;
                mg.TotalDebit = Debit;
                mg.TotalCredit = Credit;
                if (Debit > Credit)
                {
                    mg.Debtor = Debit - Credit;
                }
                else
                {
                    mg.Debtor = 0;
                }
                if (Credit > Debit)
                {
                    mg.Creditor = Credit - Debit;
                }
                else
                {
                    mg.Creditor = 0;
                }

                if (mg.MajorGeneralDetalls.Count > 0)
                {
                    mjgLit.Add(mg);
                }


            }

            return mjgLit;
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepJournals.GetById(id);

            var mapperOut = _mapper.Map<JournalIdDto>(DataSave);

            return Ok(Result<JournalIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepJournals.GetById(id);

            Data.IsActive = false;

            await RepJournals.Update(Data);

            var save = await RepJournals.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<JournalIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<JournalDto>(Data);

            return Ok(Result<JournalDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] JournalDto _UpdateDto)
        {
            var UpdateData = await RepJournals.GetById(_UpdateDto.Id);
            UpdateData.Code = _UpdateDto.Code;
            UpdateData.Reference = _UpdateDto.Reference;
            UpdateData.Commentary = _UpdateDto.Commentary;
            UpdateData.Date = _UpdateDto.Date;
            var result = await RepJournals.Update(UpdateData);
            var DataSave = await RepJournals.SaveChangesAsync();
            var DataAll = await RepJournalsDetails.GetAll();
            foreach (var item in DataAll.Where(x => x.JournalId == _UpdateDto.Id).ToList())
                item.IsActive = false;


            foreach (var intRow in _UpdateDto.JournaDetails)
            {
                if (intRow.Id != null)
                {

                    var rows = await RepJournalsDetails.GetById(intRow.Id);
                    if (rows != null)
                    {
                        if (intRow.ContactId != null)
                            rows.ContactId = intRow.ContactId;
                        rows.JournalId = rows.JournalId;
                        rows.Commentary = intRow.Commentary;
                        rows.LedgerAccountId = intRow.LedgerAccountId;
                        rows.Debit = intRow.Debit;
                        rows.Credit = intRow.Credit;
                        rows.IsActive = true;
                    }

                }
                else
                {

                    var rows = new JournaDetails();
                    if (intRow.ContactId != null)
                        rows.ContactId = intRow.ContactId;

                    rows.JournalId = UpdateData.Id;
                    rows.Commentary = intRow.Commentary;
                    rows.LedgerAccountId = intRow.LedgerAccountId;
                    rows.Debit = intRow.Debit;
                    rows.Credit = intRow.Credit;
                    rows.IsActive = true;

                    var insert = await RepJournalsDetails.Insert(rows);

                }


            }



            var data = await RepJournalsDetails.SaveChangesAsync();



            return Ok(Result<Journal>.Success(UpdateData, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
