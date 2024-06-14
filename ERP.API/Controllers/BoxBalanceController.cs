using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain;
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
    public class BoxBalanceController : ControllerBase
    {
        private readonly IGenericRepository<BoxBalance> _repBoxBalance;

        private readonly IMapper _mapper;

        private int _dataSave;

        public BoxBalanceController(IGenericRepository<BoxBalance> repBoxBalance, IMapper mapper)
        {
            _repBoxBalance = repBoxBalance;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BoxBalanceDto data)
        {
            var mapper = _mapper.Map<BoxBalance>(data);
            if (data.Balance <= 0)
                return Ok(Result<BoxBalanceDto>.Fail("no tiene balance", MessageCodes.ErrorCreating));


            var ano = await _repBoxBalance.Find(x => x.MonthBalance.Year == data.MonthBalance.Year && x.IsActive == true).FirstOrDefaultAsync();

            if (ano != null)
                return Ok(Result<BoxBalanceDto>.Fail("este año esta ingresado", MessageCodes.ErrorCreating));
            var result = await _repBoxBalance.InsertAsync(mapper);

            _dataSave = await _repBoxBalance.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<BoxBalanceDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<BoxBalanceDto>(result);
            return Ok(Result<BoxBalanceDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repBoxBalance.Find(x => x.IsActive).AsQueryable()
                .ToListAsync();


            var mapperOut = _mapper.Map<BoxBalanceDto[]>(dataSave);

            return Ok(Result<BoxBalanceDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BoxBalanceDto>>), (int)HttpStatusCode.OK)]
        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {
            var getBoxBalance = _repBoxBalance.Find(x => (x.Balance.ToString().Contains(filter.Search.Trim().ToLower()))

            ).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedDate).ToList();

            int totalRecords = getBoxBalance.Count();
            var dataMaperOut = _mapper.Map<List<BoxBalanceDto>>(getBoxBalance);

            var listBoxBalance = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<BoxBalanceDto>>.Success(listBoxBalance);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repBoxBalance.GetById(id);

            var mapperOut = _mapper.Map<BoxBalanceDto>(dataSave);

            return Ok(Result<BoxBalanceDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        
        [HttpGet("GetByYear")]
        public async Task<IActionResult> GetByYear([FromQuery] DateTime year)
        {
            var dataSave = await _repBoxBalance.Find(x => x.MonthBalance.Year == year.Year && x.IsActive == true).FirstOrDefaultAsync();


            var mapperOut = _mapper.Map<BoxBalanceDto>(dataSave);

            return Ok(Result<BoxBalanceDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repBoxBalance.GetById(id);

            data.IsActive = false;

            await _repBoxBalance.Update(data);

            var save = await _repBoxBalance.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<BoxBalanceDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<BoxBalanceDto>(data);

            return Ok(Result<BoxBalanceDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BoxBalanceDto updateDto)
        {

            if (updateDto.Balance <= 0)
                return Ok(Result<BoxBalanceDto>.Fail("no tiene balance", MessageCodes.ErrorCreating));


            var ano = await _repBoxBalance.Find(x => x.MonthBalance.Year == updateDto.MonthBalance.Year && x.IsActive == true).FirstOrDefaultAsync();

            if (ano != null)
                return Ok(Result<BoxBalanceDto>.Fail("este año esta ingresado", MessageCodes.ErrorCreating));

            var mapper = _mapper.Map<BoxBalance>(updateDto);

            mapper.IsActive = true;
            var result = await _repBoxBalance.Update(mapper);

            var dataSave = await _repBoxBalance.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<BoxBalanceDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<BoxBalanceDto>(result);

            return Ok(Result<BoxBalanceDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}