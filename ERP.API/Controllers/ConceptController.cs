using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Model.Dtos;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            var result = await RepConcepts.InsertAsync(mapper);

            var DataSave = await RepConcepts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ConceptDto>(result);

            return Ok(Result<ConceptDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var Filter = await RepConcepts.Find(x => x.IsActive == true).OrderBy(x=> x.Description).ToListAsync();

            var mapperOut = _mapper.Map<ConceptDto[]>(Filter);

            return Ok(Result<ConceptDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ConceptDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepConcepts.Find(x =>  (x.Description.ToLower().Contains(filter.Search.Trim().ToLower()))
              || (x.Reference.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Where(x => x.IsActive == true  ).Take(filter.PageSize).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<ConceptDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ConceptDto>>.Success(List);
            return Ok(Result);



        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConcepts.GetById(id);

            var mapperOut = _mapper.Map<ConceptDto>(DataSave);

            return Ok(Result<ConceptDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepConcepts.GetById(id);

            Data.IsActive = false;

            await RepConcepts.Update(Data);

            var save = await RepConcepts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ConceptDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ConceptDto>(Data);

            return Ok(Result<ConceptDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConceptDto _UpdateDto)
        {

            var mapper = _mapper.Map<Concept>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConcepts.Update(mapper);

            var DataSave = await RepConcepts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConceptDto>(result);

            return Ok(Result<ConceptDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
