using AutoMapper;

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
    public class TaxesController : ControllerBase
    {
        public readonly IGenericRepository<Taxes> RepTaxess;

        private readonly IMapper _mapper;
        public TaxesController(IGenericRepository<Taxes> repTaxess, IMapper mapper)
        {
            RepTaxess = repTaxess;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TaxesDto data)
        {
            var mapper = _mapper.Map<Taxes>(data);

            var result = await RepTaxess.Insert(mapper);

            var DataSave = await RepTaxess.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TaxesIdDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TaxesIdDto>(result);

            return Ok(Result<TaxesIdDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepTaxess.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<TaxesIdDto[]>(Filter);

            return Ok(Result<TaxesIdDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTaxess.GetById(id);

            var mapperOut = _mapper.Map<TaxesIdDto>(DataSave);

            return Ok(Result<TaxesIdDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTaxess.GetById(id);

            Data.IsActive = false;

            await RepTaxess.Update(Data);

            var save = await RepTaxess.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TaxesIdDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TaxesIdDto>(Data);

            return Ok(Result<TaxesIdDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TaxesIdDto _UpdateDto)
        {

            var mapper = _mapper.Map<Taxes>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTaxess.Update(mapper);

            var DataSave = await RepTaxess.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TaxesIdDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TaxesIdDto>(result);

            return Ok(Result<TaxesIdDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
