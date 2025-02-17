﻿
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
    public class PayrollController : ControllerBase
    {
        public readonly IGenericRepository<Payroll> RepPayrolls;

        private readonly IMapper _mapper;
        public PayrollController(IGenericRepository<Payroll> repPayrolls, IMapper mapper)
        {
            RepPayrolls = repPayrolls;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PayrollDto data)
        {
            var mapper = _mapper.Map<Payroll>(data);

            var result = await RepPayrolls.InsertAsync(mapper);

            var DataSave = await RepPayrolls.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PayrollDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<PayrollDto>(result);

            return Ok(Result<PayrollDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepPayrolls.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<PayrollDto[]>(Filter);

            return Ok(Result<PayrollDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepPayrolls.GetById(id);

            var mapperOut = _mapper.Map<PayrollDto>(DataSave);

            return Ok(Result<PayrollDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var Data = await RepPayrolls.GetById(id);

            Data.IsActive = false;

            await RepPayrolls.Update(Data);

            var save = await RepPayrolls.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<PayrollDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<PayrollDto>(Data);

            return Ok(Result<PayrollDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] PayrollDto _UpdateDto)
        {
            var mapper = _mapper.Map<Payroll>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepPayrolls.Update(mapper);

            var DataSave = await RepPayrolls.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<PayrollDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<PayrollDto>(result);

            return Ok(Result<PayrollDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
