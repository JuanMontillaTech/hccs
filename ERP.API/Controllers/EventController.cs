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
    public class EventController : ControllerBase
    {
        private readonly IGenericRepository<Event> _repEvent;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public EventController(IGenericRepository<Event> repEvent, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repEvent = repEvent;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EventDto data)
        {
            var mapper = _mapper.Map<Event>(data);


            var result = await _repEvent.Insert(mapper);
            try
            {
                _dataSave = await _repEvent.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<EventDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<EventDto>(result);
                return Ok(Result<EventDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<EventDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
            var dataSave = await _repEvent.Find(x => x.IsActive).AsQueryable().OrderByDescending(x => x.StartTime).ToListAsync();

            var mapperOut = _mapper.Map<EventDto[]>(dataSave);

            return Ok(Result<EventDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repEvent.GetById(id);

            var mapperOut = _mapper.Map<EventDto>(dataSave);

            return Ok(Result<EventDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repEvent.GetById(id);

            data.IsActive = false;

            await _repEvent.Update(data);

            var save = await _repEvent.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<EventDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<EventDto>(data);

            return Ok(Result<EventDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EventDto updateDto)
        {
            var mapper = _mapper.Map<Event>(updateDto);
            mapper.IsActive = true;
            var result = await _repEvent.Update(mapper);

            var dataSave = await _repEvent.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<EventDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<EventDto>(result);

            return Ok(Result<EventDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}