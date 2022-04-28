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




        [HttpGet("SemesterFirst")]
        public async Task<IActionResult> SemesterFirst()
        {
            try
            {
                var companyData = await RepCompany.GetAll(); var company = companyData.FirstOrDefault();
                SemesterDto semester = new SemesterDto();
                semester.Company = _mapper.Map<CompanyDto>(company);
                List<SemesterDetailsDto> semesterDetails = new List<SemesterDetailsDto>();
                string Criterion = "1";
                string Code = "SM";
                await getSemester(semesterDetails, Criterion, Code);
                semester.Icome = semesterDetails;


                List<SemesterDetailsDto> smpout = new List<SemesterDetailsDto>();
               
                await getSemester(smpout, "2", "SM");
                semester.Expense = smpout;

                return Ok(Result<SemesterDto>.Success(semester, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
    
               return Ok(Result<string>.Fail(ex.Message,ex.Message));
            } 

        }

        private async Task getSemester(List<SemesterDetailsDto> semesterDetails, string Criterion, string Code)
        {
            for (int Month = 1; Month < 7; Month++)
            {
                var semesterIncommin = await GetSemesterDetalis(Criterion, Code, Month);
                SemesterDetailsDto AddsemesterDetail = new SemesterDetailsDto();
                AddsemesterDetail = semesterIncommin;
                switch (Month)
                {
                    case 1: semesterIncommin.Month = "Enero"; break;
                    case 2: semesterIncommin.Month = "Febrero"; break;
                    case 3: semesterIncommin.Month = "Marzo"; break;
                    case 4: semesterIncommin.Month = "Abril"; break;
                    case 5: semesterIncommin.Month = "Mayo"; break;
                    case 6: semesterIncommin.Month = "Junio"; break;
                    case 7: semesterIncommin.Month = "Julio"; break;
                    case 8: semesterIncommin.Month = "Agosto"; break;
                    case 9: semesterIncommin.Month = "Septiembre"; break;
                    case 10: semesterIncommin.Month = "Octubre"; break;
                    case 11: semesterIncommin.Month = "Noviembre"; break;
                    case 12: semesterIncommin.Month = "Diciembre"; break;
                    default: semesterIncommin.Month = "Diciembre"; break;
                }


                semesterDetails.Add(semesterIncommin);
            }
        }

        private async Task<SemesterDetailsDto> GetSemesterDetalis(string Criterion, string Code, int Month)
        {
            var reportConfigures = await RepConfigurationReport.GetAll();

            SemesterDetailsDto semesterDetailsDto = new SemesterDetailsDto();
            List<MajorGeneralDto> mdtlist = new List<MajorGeneralDto>();
            foreach (var Icome in reportConfigures.Where(x => x.Code == Code && x.Criterion == Criterion).ToList())
            {
              
                    var accountBalance = await GetBalanceAccount(Guid.Parse(Icome.Parameter.ToString()), Month);
                    MajorGeneralDto majorGeneralDto = new MajorGeneralDto();
                    majorGeneralDto  = _mapper.Map<MajorGeneralDto>(accountBalance);
                    mdtlist.Add(majorGeneralDto);
                    //semesterDetailsDto.Month = 1.ToString();
                    //if (accountBalance.TotalCredit > 0)
                    //{
                    //    semesterDetailsDto.Total = accountBalance.TotalCredit;
                    //}
                    //else
                    //{
                    //    semesterDetailsDto.Total = accountBalance.TotalDebit;
                    //}

             


            }

            semesterDetailsDto.Account =mdtlist;
            return semesterDetailsDto;

        }

        [HttpGet("SemesterSecond")]
        public async Task<IActionResult> SemesterSecond()
        {
            List<MajorGeneralDto> mjgLit = await GetGeneralMajor();



            return Ok(Result<List<MajorGeneralDto>>.Success(mjgLit, MessageCodes.AllSuccessfully()));


        }

        private async Task<MajorGeneralDto> GetBalanceAccount(Guid AccountId, int Month)
        {
            var Account = await RepLedgerAccounts.GetById(AccountId);
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
