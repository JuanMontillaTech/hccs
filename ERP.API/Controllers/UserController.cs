﻿using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IGenericRepository<Sys_User> _repository;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IGenericRepository<Sys_User> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Sys_UserDto data)
        {
            var mapper = _mapper.Map<Sys_User>(data);


            var result = await _repository.Insert(mapper);
            try
            {
                var DataSave = await _repository.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<Sys_UserDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<Sys_UserDto>(result);
                return Ok(Result<Sys_UserDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<Sys_UserDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {


            try
            {

                var DataSave = await _repository.Find(x => x.IsActive).AsQueryable()
              .ToListAsync();


                var mapperOut = _mapper.Map<Sys_UserDto[]>(DataSave);

                return Ok(Result<Sys_UserDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<Sys_UserDto[]>.Fail(MessageCodes.BabData(), ex.Message));
            }

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await _repository.GetById(id);

            var mapperOut = _mapper.Map<Sys_UserDto>(DataSave);

            return Ok(Result<Sys_UserDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await _repository.GetById(id);

            Data.IsActive = false;

            await _repository.Update(Data);

            var save = await _repository.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<Sys_UserDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<Sys_UserDto>(Data);

            return Ok(Result<Sys_UserDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Sys_UserDto _UpdateDto)
        {

            var mapper = _mapper.Map<Sys_User>(_UpdateDto);
            mapper.IsActive = true;
            var result = await _repository.Update(mapper);

            var DataSave = await _repository.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<Sys_UserDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<Sys_UserDto>(result);

            return Ok(Result<Sys_UserDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}