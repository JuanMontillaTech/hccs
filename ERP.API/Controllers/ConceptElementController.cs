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
    public class ConceptElementController : ControllerBase
    {
        public readonly IGenericRepository<ConceptElement> RepConceptElements;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConceptElementController(IGenericRepository<ConceptElement> repConceptElements, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepConceptElements = repConceptElements;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ConceptElementDto data)
        {
            var mapper = _mapper.Map<ConceptElement>(data);

            var result = await RepConceptElements.InsertAsync(mapper);

            var DataSave = await RepConceptElements.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptElementDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<ConceptElementDto>(result);

            return Ok(Result<ConceptElementDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var Filter = await RepConceptElements.Find(x => x.IsActive == true).Include(x => x.Concepts).OrderBy(x=> x.Name).ToListAsync();

            var mapperOut = _mapper.Map<ConceptElementDto[]>(Filter);

            return Ok(Result<ConceptElementDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ConceptElementDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepConceptElements.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             //|| (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))            
            ).Include(x=> x.Concepts).ToList();

            int totalRecords = Filter.Count();
            var DataMaperOut = _mapper.Map<List<ConceptElementDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ConceptElementDto>>.Success(List);
            return Ok(Result);



        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepConceptElements.GetById(id);

            var mapperOut = _mapper.Map<ConceptElementDto>(DataSave);

            return Ok(Result<ConceptElementDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
     
        [HttpGet("GetByConcepId")]
        public async Task<IActionResult> GetByConcepId([FromQuery] Guid id)
        {
            var DataSave = await RepConceptElements.Find(x=> x.ConceptId == id).ToListAsync();

            var mapperOut = _mapper.Map<ConceptElementDto[]>(DataSave);

            return Ok(Result<ConceptElementDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepConceptElements.GetById(id);

            Data.IsActive = false;

            await RepConceptElements.Update(Data);

            var save = await RepConceptElements.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ConceptElementDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ConceptElementDto>(Data);

            return Ok(Result<ConceptElementDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConceptElementDto _UpdateDto)
        {

            var mapper = _mapper.Map<ConceptElement>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepConceptElements.Update(mapper);

            var DataSave = await RepConceptElements.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ConceptElementDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ConceptElementDto>(result);

            return Ok(Result<ConceptElementDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
