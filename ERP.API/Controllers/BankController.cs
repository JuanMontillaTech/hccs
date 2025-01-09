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

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankServices _bankServices;
        private readonly IMapper _mapper;

        public BankController(IBankServices bankServices, IMapper mapper)
        {
            _bankServices = bankServices;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BankDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = _mapper.Map<Banks>(data);
            var result = await _bankServices.InsertBankAccount(mapper);

            if (result == null)
            {
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorCreating, "API"));
            }

            var mapperOut = _mapper.Map<BankDto>(result);
            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _bankServices.GetAll();
            var mapperOut = _mapper.Map<BankDetallisDto[]>(dataSave);
            return Ok(Result<BankDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<BankDetallisDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFilter([FromQuery] PaginationFilter filter)
        {
            var getBanks = await _bankServices.FindBankAccount(filter);
            int totalRecords = getBanks.Count();
            var dataMaperOut = _mapper.Map<List<BankDetallisDto>>(getBanks);

            var listBanks = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<BankDetallisDto>>.Success(listBanks);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _bankServices.GetBankAccountById(id);
            if (dataSave == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra
            }
            var mapperOut = _mapper.Map<BankDto>(dataSave);
            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _bankServices.DeleteBankAccount(id);

            if (data == null)
            {
                return NotFound(); // Devuelve 404 si no se encuentra
            }

            var mapperOut = _mapper.Map<BankDto>(data);
            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BankDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapper = _mapper.Map<Banks>(updateDto);
            var result = await _bankServices.UpdateBankAccount(mapper);

            if (result == null)
            {
                return Ok(Result<BankDto>.Fail(MessageCodes.ErrorUpdating, "API"));
            }

            var mapperOut = _mapper.Map<BankDto>(result);
            return Ok(Result<BankDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}