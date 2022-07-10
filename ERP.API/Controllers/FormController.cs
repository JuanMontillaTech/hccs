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
    public class FormController : ControllerBase
    {
        public readonly IGenericRepository<Form> RepForms;

        private readonly IMapper _mapper;
        public FormController(IGenericRepository<Form> repForms, IMapper mapper)
        {
            RepForms = repForms;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormDto data)
        {
            var mapper = _mapper.Map<Form>(data);

            var result = await RepForms.Insert(mapper);

            var DataSave = await RepForms.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<FormDto>(result);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepForms.GetAll();

            var Filter = DataSave.Where(x => x.IsActive == true).ToList();

            var mapperOut = _mapper.Map<List<FormDto>>(Filter);

            return Ok(Result<List<FormDto>>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            var DataSave = await RepForms.GetById(id);

            var mapperOut = _mapper.Map<FormDto>(DataSave);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepForms.GetById(id);

            Data.IsActive = false;

            await RepForms.Update(Data);

            var save = await RepForms.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormDto>(Data);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormDto _UpdateDto)
        {

            var mapper = _mapper.Map<Form>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepForms.Update(mapper);

            var DataSave = await RepForms.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormDto>(result);

            return Ok(Result<FormDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
