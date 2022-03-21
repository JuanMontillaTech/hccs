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
        public readonly INumerationService numerationService;

       
        private readonly IMapper _mapper;
        public JournalController(IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails, IMapper mapper ,
        INumerationService numerationService)
        {
            this.numerationService = numerationService;
            RepJournals = repJournals;
            RepJournalsDetails = repJournalsDetails;
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
            var DataFillter = DataSave.Where(x => x.IsActive == true ).ToList();
            foreach (var item in DataFillter)
            {
                item.JournaDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.JournalId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Journal>>.Success(DataFillter, MessageCodes.AllSuccessfully()));
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
                    if (rows != null){
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
