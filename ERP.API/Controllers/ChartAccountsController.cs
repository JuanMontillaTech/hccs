using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartAccountsController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMapper _mapper;
    private readonly IGenericRepository<LedgerAccount> _repLedgerAccountDto;

    public ChartAccountsController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IGenericRepository<LedgerAccount> repLedgerAccountDto)
    {
        _httpContextAccessor = httpContextAccessor;
        _repLedgerAccountDto = repLedgerAccountDto;
        _mapper = mapper;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] LedgerAccountDto data)
    {
        var mapper = _mapper.Map<LedgerAccount>(data);

        var result = await _repLedgerAccountDto.InsertAsync(mapper);

        var dataSave = await _repLedgerAccountDto.SaveChangesAsync();

        if (dataSave != 1)
            return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorCreating, "API"));
        var mapperOut = _mapper.Map<LedgerAccountDto>(result);

        return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var filter = await _repLedgerAccountDto.Find(x => x.IsActive == true).OrderBy(x => x.Code).ToListAsync();

        var mapperOut = _mapper.Map<LedgerAccountDto[]>(filter);

        return Ok(Result<LedgerAccountDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }

    [HttpGet("GetFilter")]
    [ProducesResponseType(typeof(Result<ICollection<LedgerAccountDto>>), (int)HttpStatusCode.OK)]
    public IActionResult GetFilter([FromQuery] PaginationFilter filter)
    {
        var Filter = _repLedgerAccountDto.Find(x => x.Code.ToLower().Contains(filter.Search.Trim().ToLower())
                                                    || x.Name.ToLower().Contains(filter.Search.Trim().ToLower())
        ).Where(x => x.IsActive == true).Take(filter.PageSize).ToList();

        var totalRecords = Filter.Count();
        var dataMaperOut = _mapper.Map<List<LedgerAccountDto>>(Filter);

        var list = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords) ??
                   throw new ArgumentNullException("dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords)");
        var result = Result<PagesPagination<LedgerAccountDto>>.Success(list);
        return Ok(result);
    }


    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var dataSave = await _repLedgerAccountDto.GetById(id);

        var mapperOut = _mapper.Map<LedgerAccountDto>(dataSave);

        return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Data = await _repLedgerAccountDto.GetById(id);

        Data.IsActive = false;

        await _repLedgerAccountDto.Update(Data);

        var save = await _repLedgerAccountDto.SaveChangesAsync();

        if (save != 1)
            return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorDeleting, "API"));

        var mapperOut = _mapper.Map<LedgerAccountDto>(Data);

        return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] LedgerAccountDto updateDto)
    {
        var mapper = _mapper.Map<LedgerAccount>(updateDto);
        mapper.IsActive = true;
        var result = await _repLedgerAccountDto.Update(mapper);

        var dataSave = await _repLedgerAccountDto.SaveChangesAsync();

        if (dataSave != 1)
            return Ok(Result<LedgerAccountDto>.Fail(MessageCodes.ErrorUpdating, "API"));

        var mapperOut = _mapper.Map<LedgerAccountDto>(result);

        return Ok(Result<LedgerAccountDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
    }
}