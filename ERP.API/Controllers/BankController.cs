
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
using ERP.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class BankController : ControllerBase
    {
        public readonly IGenericRepository<Banks> RepBanks;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BankController(IGenericRepository<Banks> repBanks, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepBanks = repBanks;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankDto data)
        {
            var mapper = _mapper.Map<Banks>(data);


            var result = await RepBanks.Insert(mapper);
            try
            {
                var DataSave = await RepBanks.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<BankDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<BankDto>(result);
                return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<BankDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepBanks.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Currencys).Include(x=> x.LedgerAccount).ToListAsync();
             

            var mapperOut = _mapper.Map<BankDetallisDto[]>(DataSave);

            return Ok(Result<BankDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepBanks.GetById(id);

            var mapperOut = _mapper.Map<BankDto>(DataSave);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepBanks.GetById(id);

            Data.IsActive = false;

            await RepBanks.Update(Data);

            var save = await RepBanks.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<BankDto>(Data);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankDto _UpdateDto)
        {

            var mapper = _mapper.Map<Banks>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepBanks.Update(mapper);

            var DataSave = await RepBanks.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<BankDto>(result);

            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
