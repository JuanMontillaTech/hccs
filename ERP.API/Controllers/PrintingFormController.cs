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
    public class PrintingFormController : ControllerBase
    {
        private readonly IGenericRepository<PrintingForm> _repPrintingForm;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public PrintingFormController(IGenericRepository<PrintingForm> repPrintingForm, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repPrintingForm = repPrintingForm;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PrintingFormDto data)
        {
            var mapper = _mapper.Map<PrintingForm>(data);

 
            var result = await _repPrintingForm.Insert(mapper);
            try
            {
                _dataSave = await _repPrintingForm.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<PrintingFormDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<PrintingFormDto>(result);
                return Ok(Result<PrintingFormDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<PrintingFormDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repPrintingForm.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Forms).Include(x => x.PrintingTemplates).ToListAsync();


            var mapperOut = _mapper.Map<PrintingFormDto[]>(dataSave);

            return Ok(Result<PrintingFormDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repPrintingForm.GetById(id);

            var mapperOut = _mapper.Map<PrintingFormDto>(dataSave);

            return Ok(Result<PrintingFormDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repPrintingForm.GetById(id);

            data.IsActive = false;

            await _repPrintingForm.Update(data);

            var save = await _repPrintingForm.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<PrintingFormDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<PrintingFormDto>(data);

            return Ok(Result<PrintingFormDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PrintingFormDto updateDto)
        {
            var mapper = _mapper.Map<PrintingForm>(updateDto);
            mapper.IsActive = true;
            var result = await _repPrintingForm.Update(mapper);

            var dataSave = await _repPrintingForm.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<PrintingFormDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<PrintingFormDto>(result);

            return Ok(Result<PrintingFormDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}