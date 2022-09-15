using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
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
        private readonly IGenericRepository<PrintingTemplate> _repPrintingTemplate;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public PrintingTemplateController(IGenericRepository<PrintingTemplate> repPrintingTemplate, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repPrintingTemplate = repPrintingTemplate;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PrintingTemplateDto data)
        {
            var mapper = _mapper.Map<PrintingTemplate>(data);


            var result = await _repPrintingTemplate.Insert(mapper);
            try
            {
                _dataSave = await _repPrintingTemplate.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<PrintingTemplateDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<PrintingTemplateDto>(result);
                return Ok(Result<PrintingTemplateDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<PrintingTemplateDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repPrintingTemplate.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<PrintingTemplateDto[]>(dataSave);

            return Ok(Result<PrintingTemplateDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repPrintingTemplate.GetById(id);

            var mapperOut = _mapper.Map<PrintingTemplateDto>(dataSave);

            return Ok(Result<PrintingTemplateDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repPrintingTemplate.GetById(id);

            data.IsActive = false;

            await _repPrintingTemplate.Update(data);

            var save = await _repPrintingTemplate.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<PrintingTemplateDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<PrintingTemplateDto>(data);

            return Ok(Result<PrintingTemplateDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PrintingTemplateDto updateDto)
        {
            var mapper = _mapper.Map<PrintingTemplate>(updateDto);
            mapper.IsActive = true;
            var result = await _repPrintingTemplate.Update(mapper);

            var dataSave = await _repPrintingTemplate.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<PrintingTemplateDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<PrintingTemplateDto>(result);

            return Ok(Result<PrintingTemplateDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}