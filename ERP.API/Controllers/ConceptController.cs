using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
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
    public class ConceptController : ControllerBase
    {
        public readonly IGenericRepository<Concept> RepConcepts;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConceptController(IGenericRepository<Concept> repConcepts, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepConcepts = repConcepts;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ConceptDto data)
        {
            var mapper = _mapper.Map<Concept>(data);

            var result = await RepConcepts.Insert(mapper);

            var DataSave = await RepConcepts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ConceptIdDto>(result);

            return Ok(Result<ConceptIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepConcepts.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<ConceptIdDto[]>(Filter);

            return Ok(Result<ConceptIdDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConcepts.GetById(id);

            var mapperOut = _mapper.Map<ConceptIdDto>(DataSave);

            return Ok(Result<ConceptIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepConcepts.GetById(id);

            Data.IsActive = false;

            await RepConcepts.Update(Data);

            var save = await RepConcepts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ConceptIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ConceptIdDto>(Data);

            return Ok(Result<ConceptIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConceptIdDto _UpdateDto)
        {

            var mapper = _mapper.Map<Concept>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConcepts.Update(mapper);

            var DataSave = await RepConcepts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConceptIdDto>(result);

            return Ok(Result<ConceptIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
