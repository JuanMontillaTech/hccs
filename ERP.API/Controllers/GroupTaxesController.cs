using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ERP.API.ValidatorDto;
using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupTaxesController : ControllerBase
    {
        private readonly IGenericRepository<GroupTaxes> _repGroupTaxes;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public GroupTaxesController(IGenericRepository<GroupTaxes> repGroupTaxes, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repGroupTaxes = repGroupTaxes;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GroupTaxesDto data)
        {
 

            var mapper = _mapper.Map<GroupTaxes>(data);


            var result = await _repGroupTaxes.InsertAsync(mapper);
            try
            {
                _dataSave = await _repGroupTaxes.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<GroupTaxesDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<GroupTaxesDto>(result);
                return Ok(Result<GroupTaxesDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<GroupTaxesDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repGroupTaxes.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<GroupTaxesDto[]>(dataSave);

            return Ok(Result<GroupTaxesDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<GroupTaxesDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repGroupTaxes.Find(x => x.IsActive == true
      && (x.Description.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repGroupTaxes.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<GroupTaxesDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<GroupTaxesDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repGroupTaxes.GetById(id);

            var mapperOut = _mapper.Map<GroupTaxesDto>(dataSave);

            return Ok(Result<GroupTaxesDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repGroupTaxes.GetById(id);

            data.IsActive = false;

            await _repGroupTaxes.Update(data);

            var save = await _repGroupTaxes.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<GroupTaxesDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<GroupTaxesDto>(data);

            return Ok(Result<GroupTaxesDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] GroupTaxesDto updateDto)
        {
            var mapper = _mapper.Map<GroupTaxes>(updateDto);
            mapper.IsActive = true;
            var result = await _repGroupTaxes.Update(mapper);

            var dataSave = await _repGroupTaxes.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<GroupTaxesDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<GroupTaxesDto>(result);

            return Ok(Result<GroupTaxesDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}