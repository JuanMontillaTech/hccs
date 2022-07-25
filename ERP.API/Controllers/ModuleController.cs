
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
    public class ModuleController : ControllerBase
    {
        public readonly IGenericRepository<Module> RepModule;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ModuleController(IGenericRepository<Module> repModule, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepModule = repModule;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ModuleDto data)
        {
            var mapper = _mapper.Map<Module>(data);


            var result = await RepModule.Insert(mapper);
            try
            {
                var DataSave = await RepModule.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<ModuleDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<ModuleDto>(result);
                return Ok(Result<ModuleDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<ModuleDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepModule.Find(x => x.IsActive).AsQueryable()
               .ToListAsync();
             

            var mapperOut = _mapper.Map<ModuleDto[]>(DataSave);

            return Ok(Result<ModuleDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepModule.GetById(id);

            var mapperOut = _mapper.Map<ModuleDto>(DataSave);

            return Ok(Result<ModuleDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepModule.GetById(id);

            Data.IsActive = false;

            await RepModule.Update(Data);

            var save = await RepModule.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<ModuleDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<ModuleDto>(Data);

            return Ok(Result<ModuleDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ModuleDto _UpdateDto)
        {

            var mapper = _mapper.Map<Module>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepModule.Update(mapper);

            var DataSave = await RepModule.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<ModuleDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<ModuleDto>(result);

            return Ok(Result<ModuleDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
