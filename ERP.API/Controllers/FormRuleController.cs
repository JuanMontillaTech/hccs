using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.API.ValidatorDto;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormRuleController : ControllerBase
    {
        private readonly ISysRepository<FormRule> _repFormRule;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public FormRuleController(ISysRepository<FormRule> repFormRule, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repFormRule = repFormRule;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormRuleDto data)
        {
 

            var mapper = _mapper.Map<FormRule>(data);


            var result = await _repFormRule.InsertAsync(mapper);
            try
            {
                _dataSave = await _repFormRule.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<FormRuleDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<FormRuleDto>(result);
                return Ok(Result<FormRuleDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<FormRuleDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repFormRule.Find(x => x.IsActive).AsQueryable().Include(x => x.Froms).ToListAsync();


            var mapperOut = _mapper.Map<FormRuleDto[]>(dataSave);

            return Ok(Result<FormRuleDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<FormRuleDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repFormRule.Find(x => x.IsActive == true
      && (x.Rule.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Include(x=> x.Froms).ToList();

            int totalRecords = _repFormRule.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<FormRuleDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<FormRuleDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repFormRule.GetById(id);

            var mapperOut = _mapper.Map<FormRuleDto>(dataSave);

            return Ok(Result<FormRuleDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
      
        [HttpGet("GetRulesByFromId/{FormId}")]
        public async Task<IActionResult> GetRulesByFromId(Guid FormId)
        {
            var dataSave = await _repFormRule.Find(x=> x.IsActive).Where(x=> x.FormId == FormId).Include(x=>x.Froms).ToListAsync();

            var mapperOut = _mapper.Map<FormRuleDto[]>(dataSave);

            return Ok(Result<FormRuleDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repFormRule.GetById(id);

            data.IsActive = false;

            await _repFormRule.Update(data);

            var save = await _repFormRule.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormRuleDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormRuleDto>(data);

            return Ok(Result<FormRuleDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormRuleDto updateDto)
        {
            var mapper = _mapper.Map<FormRule>(updateDto);
            mapper.IsActive = true;
            var result = await _repFormRule.Update(mapper);

            var dataSave = await _repFormRule.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<FormRuleDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormRuleDto>(result);

            return Ok(Result<FormRuleDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}