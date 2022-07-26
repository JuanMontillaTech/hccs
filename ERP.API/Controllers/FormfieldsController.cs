
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
    public class FormfieldsController : ControllerBase
    {
        public readonly IGenericRepository<Formfields> RepFormfields;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FormfieldsController(IGenericRepository<Formfields> repFormfields, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepFormfields = repFormfields;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FormfieldsDto data)
        {
            var mapper = _mapper.Map<Formfields>(data);


            var result = await RepFormfields.Insert(mapper);
            try
            {
                var DataSave = await RepFormfields.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<FormfieldsDto>(result);
                return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<FormfieldsDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepFormfields.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Froms).ToListAsync();
             

            var mapperOut = _mapper.Map<FormfieldsDetallisDto[]>(DataSave);

            return Ok(Result<FormfieldsDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepFormfields.GetById(id);

            var mapperOut = _mapper.Map<FormfieldsDto>(DataSave);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetByFormId/{id}")]
        public async Task<IActionResult> GetByFormId( Guid id)
        {
            try
            {

                var DataSave = await RepFormfields.Find(x => x.IsActive && x.FormId == id).AsQueryable()
                    .Include(x => x.Froms).OrderBy(x => x.ColumnIndex).ToListAsync();


                var mapperOut = _mapper.Map<FormfieldsDetallisDto[]>(DataSave);

                return Ok(Result<FormfieldsDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<FormfieldsDetallisDto[]>.Fail(MessageCodes.BabData(),ex.Message));
            }
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepFormfields.GetById(id);

            Data.IsActive = false;

            await RepFormfields.Update(Data);

            var save = await RepFormfields.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<FormfieldsDto>(Data);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FormfieldsDto _UpdateDto)
        {

            var mapper = _mapper.Map<Formfields>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepFormfields.Update(mapper);

            var DataSave = await RepFormfields.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<FormfieldsDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<FormfieldsDto>(result);

            return Ok(Result<FormfieldsDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
