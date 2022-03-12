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

        private readonly IMapper _mapper;
        public JournalController(IGenericRepository<Journal> repJournals,
        IGenericRepository<JournaDetails> repJournalsDetails, IMapper mapper)
        {
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
                var result = await RepJournals.Insert(mapper);

                var DataSave = await RepJournals.SaveChangesAsync();

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

            foreach (var item in DataSave)
            {
                item.JournaDetails = DataSaveDetails.AsQueryable()
                     .Where(x => x.IsActive == true && x.JournalId == item.Id).ToList();

            }

            return Ok(Result<IEnumerable<Journal>>.Success(DataSave, MessageCodes.AllSuccessfully()));
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

            var mapperOut = _mapper.Map<JournalIdDto>(Data);

            return Ok(Result<JournalIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Journal _UpdateDto)
        {
            var UpdateData = await RepJournals.GetById(_UpdateDto.Id);
            UpdateData.Code = _UpdateDto.Code;
            UpdateData.Reference = _UpdateDto.Reference;
            UpdateData.Commentary = _UpdateDto.Commentary;
            UpdateData.Date = _UpdateDto.Date;



            var result = await RepJournals.Update(UpdateData);



            var DataSave = await RepJournals.SaveChangesAsync();


            var DataSaveDetails = await RepJournalsDetails.GetAll();

            foreach (var item in DataSaveDetails.Where(x=> x.JournalId == _UpdateDto.Id))
            {
                foreach (var intRow in _UpdateDto.JournaDetails)
                {
                    if(item.Id == intRow.Id){

                        item.ContactId = item.ContactId;
                        item.Commentary = item.Commentary;
                        item.LedgerAccountId = item.LedgerAccountId;
                        item.Debit = item.Debit;
                        item.Credit = item.Credit; 
                    }else{
                        item.IsActive = false;
                    }

                }


            }
             var data = await RepJournalsDetails.SaveChangesAsync();

 

            return Ok(Result<Journal>.Success(UpdateData, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
