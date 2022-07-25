
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
    public class UserRollController : ControllerBase
    {
        public readonly IGenericRepository<UserRoll> RepUserRoll;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRollController(IGenericRepository<UserRoll> repUserRoll, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepUserRoll = repUserRoll;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserRollDto data)
        {
            var mapper = _mapper.Map<UserRoll>(data);


            var result = await RepUserRoll.Insert(mapper);
            try
            {
                var DataSave = await RepUserRoll.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<UserRollDto>(result);
                return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<UserRollDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var DataSave = await RepUserRoll.Find(x => x.IsActive).AsQueryable()
                .Include(x=> x.Rolles).ToListAsync();
             

            var mapperOut = _mapper.Map<UserRollDetallisDto[]>(DataSave);

            return Ok(Result<UserRollDetallisDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepUserRoll.GetById(id);

            var mapperOut = _mapper.Map<UserRollDto>(DataSave);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepUserRoll.GetById(id);

            Data.IsActive = false;

            await RepUserRoll.Update(Data);

            var save = await RepUserRoll.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<UserRollDto>(Data);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserRollDto _UpdateDto)
        {

            var mapper = _mapper.Map<UserRoll>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepUserRoll.Update(mapper);

            var DataSave = await RepUserRoll.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<UserRollDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<UserRollDto>(result);

            return Ok(Result<UserRollDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
