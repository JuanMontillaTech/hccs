using AutoMapper;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IGenericRepository<SysUser> _repository;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IGenericRepository<SysUser> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Sys_UserDto data)
        {
            var mapper = _mapper.Map<SysUser>(data);


            var result = await _repository.InsertAsync(mapper);
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
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<Sys_UserDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repository.Find(x => x.IsActive == true
            && (x.Email.ToLower().Contains(filter.Search.Trim().ToLower()))
            && (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = _repository.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<Sys_UserDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<Sys_UserDto>>.Success(List);
            return Ok(Result);

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

            var mapper = _mapper.Map<SysUser>(_UpdateDto);
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
