﻿using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Model.Dtos;
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
    public class CurrencyController : ControllerBase
    {
        public readonly IGenericRepository<Currency> RepCurrencys;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrencyController(IGenericRepository<Currency> repCurrencys, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepCurrencys = repCurrencys;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CurrencyDto data)
        {
            var mapper = _mapper.Map<Currency>(data);

            var result = await RepCurrencys.Insert(mapper);

            var DataSave = await RepCurrencys.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<CurrencyDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<CurrencyDto>(result);

            return Ok(Result<CurrencyDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepCurrencys.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<CurrencyDto[]>(Filter);

            return Ok(Result<CurrencyDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepCurrencys.GetById(id);

            var mapperOut = _mapper.Map<CurrencyDto>(DataSave);

            return Ok(Result<CurrencyDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepCurrencys.GetById(id);

            Data.IsActive = false;

            await RepCurrencys.Update(Data);

            var save = await RepCurrencys.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<CurrencyDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<CurrencyDto>(Data);

            return Ok(Result<CurrencyDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CurrencyDto _UpdateDto)
        {

            var mapper = _mapper.Map<Currency>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepCurrencys.Update(mapper);

            var DataSave = await RepCurrencys.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<CurrencyDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<CurrencyDto>(result);

            return Ok(Result<CurrencyDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}