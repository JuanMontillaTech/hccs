using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class LedgerAccountController : ControllerBase
    {
        public readonly IGenericRepository<LedgerAccount> RepLedgerAccounts;

        private readonly IMapper _mapper;
        public LedgerAccountController(IGenericRepository<LedgerAccount> repLedgerAccounts, IMapper mapper)
        {
            RepLedgerAccounts = repLedgerAccounts;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] LedgerAccountDto data)
        {
            var mapper = _mapper.Map<LedgerAccount>(data);

            var result = await RepLedgerAccounts.Insert(mapper);

            var DataSave = await RepLedgerAccounts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<LedgerAccountIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<LedgerAccountIdDto>(result);

            return Ok(Result<LedgerAccountIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepLedgerAccounts.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<LedgerAccountIdDto[]>(Filter);

            return Ok(Result<LedgerAccountIdDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepLedgerAccounts.GetById(id);

            var mapperOut = _mapper.Map<LedgerAccountIdDto>(DataSave);

            return Ok(Result<LedgerAccountIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepLedgerAccounts.GetById(id);

            Data.IsActive = false;

            await RepLedgerAccounts.Update(Data);

            var save = await RepLedgerAccounts.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<LedgerAccountIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<LedgerAccountIdDto>(Data);

            return Ok(Result<LedgerAccountIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] LedgerAccountIdDto _UpdateDto)
        {
            var mapper = _mapper.Map<LedgerAccount>(_UpdateDto);

            var result = await RepLedgerAccounts.Update(mapper);

            var DataSave = await RepLedgerAccounts.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<LedgerAccountIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<LedgerAccountIdDto>(result);

            return Ok(Result<LedgerAccountIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
