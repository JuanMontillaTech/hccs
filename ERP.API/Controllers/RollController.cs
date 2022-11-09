
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Model.Dtos;
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
    public class RollController : ControllerBase
    {
        public readonly IGenericRepository<Roll> RepRoll;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RollController(IGenericRepository<Roll> repRoll, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepRoll = repRoll;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RollDto data)
        {
            var mapper = _mapper.Map<Roll>(data);


            var result = await RepRoll.Insert(mapper);
            try
            {
                var DataSave = await RepRoll.SaveChangesAsync();
                if (DataSave != 1)
                    return Ok(Result<RollDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<RollDto>(result);
                return Ok(Result<RollDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

               return Ok(Result<RollDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));



        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
             

            try
            {

                var DataSave = await RepRoll.Find(x => x.IsActive).AsQueryable()
              .ToListAsync();


                var mapperOut = _mapper.Map<RollDto[]>(DataSave);

                return Ok(Result<RollDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
            }
            catch (Exception ex)
            {
                return Ok(Result<RollDto[]>.Fail(MessageCodes.BabData(), ex.Message));
            }

        }

        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<RollDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepRoll.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Commentary.ToLower().Contains(filter.Search.Trim().ToLower()))

            ).ToList();

            int totalRecords = RepRoll.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<RollDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<RollDto>>.Success(List);
            return Ok(Result);

        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepRoll.GetById(id);

            var mapperOut = _mapper.Map<RollDto>(DataSave);

            return Ok(Result<RollDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepRoll.GetById(id);

            Data.IsActive = false;

            await RepRoll.Update(Data);

            var save = await RepRoll.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<RollDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<RollDto>(Data);

            return Ok(Result<RollDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] RollDto _UpdateDto)
        {

            var mapper = _mapper.Map<Roll>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepRoll.Update(mapper);

            var DataSave = await RepRoll.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<RollDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<RollDto>(result);

            return Ok(Result<RollDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
