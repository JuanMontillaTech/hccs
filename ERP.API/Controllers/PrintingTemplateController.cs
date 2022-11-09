using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintingTemplateController : ControllerBase
    {
        private readonly IGenericRepository<ReportQuery> _repPrintingTemplate;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public PrintingTemplateController(IGenericRepository<ReportQuery> repPrintingTemplate, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repPrintingTemplate = repPrintingTemplate;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ReportQueryDto data)
        {
            var mapper = _mapper.Map<ReportQuery>(data);


            var result = await _repPrintingTemplate.Insert(mapper);
            try
            {
                _dataSave = await _repPrintingTemplate.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<ReportQueryDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<ReportQueryDto>(result);
                return Ok(Result<ReportQueryDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<ReportQueryDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repPrintingTemplate.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<ReportQueryDto[]>(dataSave);

            return Ok(Result<ReportQueryDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }


        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<ReportQueryDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repPrintingTemplate.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = _repPrintingTemplate.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<ReportQueryDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<ReportQueryDto>>.Success(List);
            return Ok(Result);

        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repPrintingTemplate.GetById(id);

            var mapperOut = _mapper.Map<ReportQueryDto>(dataSave);

            return Ok(Result<ReportQueryDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repPrintingTemplate.GetById(id);

            data.IsActive = false;

            await _repPrintingTemplate.Update(data);

            var save = await _repPrintingTemplate.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ReportQueryDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ReportQueryDto>(data);

            return Ok(Result<ReportQueryDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ReportQueryDto updateDto)
        {
            var mapper = _mapper.Map<ReportQuery>(updateDto);
            mapper.IsActive = true;
            var result = await _repPrintingTemplate.Update(mapper);

            var dataSave = await _repPrintingTemplate.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<ReportQueryDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ReportQueryDto>(result);

            return Ok(Result<ReportQueryDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}