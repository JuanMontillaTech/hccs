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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly IGenericRepository<Box> _repBox;

        private readonly IMapper _mapper;

        private int _dataSave;

        public BoxController(IGenericRepository<Box> repBox, IMapper mapper)
        {
            _repBox = repBox;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BoxDto data)
        {
            var mapper = _mapper.Map<Box>(data);

            var result = await _repBox.InsertAsync(mapper);

            _dataSave = await _repBox.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<BoxDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<BoxDto>(result);
            return Ok(Result<BoxDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repBox.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Currency).Include(x => x.LedgerAccount).ToListAsync();


            var mapperOut = _mapper.Map<BoxDto[]>(dataSave);

            return Ok(Result<BoxDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BoxDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var getBox = _repBox.Find(x => x.IsActive == true
                                           && (x.Currency.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
                                           || (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repBox.Find(t => t.IsActive).Count();
            var dataMaperOut = _mapper.Map<List<BoxDto>>(getBox);

            var listBox = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<BoxDto>>.Success(listBox);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repBox.GetById(id);

            var mapperOut = _mapper.Map<BoxDto>(dataSave);

            return Ok(Result<BoxDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repBox.GetById(id);

            data.IsActive = false;

            await _repBox.Update(data);

            var save = await _repBox.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<BoxDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<BoxDto>(data);

            return Ok(Result<BoxDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BoxDto updateDto)
        {
            var mapper = _mapper.Map<Box>(updateDto);
            mapper.IsActive = true;
            var result = await _repBox.Update(mapper);

            var dataSave = await _repBox.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<BoxDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<BoxDto>(result);

            return Ok(Result<BoxDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}