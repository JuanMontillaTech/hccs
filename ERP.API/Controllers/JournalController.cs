using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;

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
            var DataDeteill = await RepJournalsDetails.GetAll();
            List<Journal> Filter = DataSave .Where(x => x.IsActive == true).ToList();
            var FilterDetails = DataDeteill .Where(x => x.IsActive == true).ToList();
            var Query =  from j in Filter
                                join dt in FilterDetails on j.Id equals dt.JournalId
                                select j ;
            var result = Query.ToList();
                             
                          
           

                        


            var mapperOut = _mapper.Map<Journal[]>(result);

            return Ok(Result<Journal[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
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
        public async Task<IActionResult> Update([FromBody] JournalDto _UpdateDto)
        {
            var mapper = _mapper.Map<Journal>(_UpdateDto);
            mapper.IsActive = false;
            var result = await RepJournals.Update(mapper);
            mapper.Id = Guid.Empty;
             await RepJournals.Insert(mapper);

            var DataSave = await RepJournals.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<JournalDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<JournalDto>(result);

            return Ok(Result<JournalDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
