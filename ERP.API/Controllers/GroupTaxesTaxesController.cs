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
    public class GroupTaxesTaxesController : ControllerBase
    {
        private readonly IGenericRepository<GroupTaxesTaxes> _repGroupTaxesTaxes;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public GroupTaxesTaxesController(IGenericRepository<GroupTaxesTaxes> repGroupTaxesTaxes, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repGroupTaxesTaxes = repGroupTaxesTaxes;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GroupTaxesTaxesDto data)
        {


            var mapper = _mapper.Map<GroupTaxesTaxes>(data);


            var result = await _repGroupTaxesTaxes.InsertAsync(mapper);
            try
            {
                _dataSave = await _repGroupTaxesTaxes.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<GroupTaxesTaxesDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<GroupTaxesTaxesDto>(result);
                return Ok(Result<GroupTaxesTaxesDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<GroupTaxesTaxesDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repGroupTaxesTaxes.Find(x => x.IsActive).AsQueryable().Include(x => x.Taxes).Include(x => x.GroupTaxes).ToListAsync();


            var mapperOut = _mapper.Map<GroupTaxesTaxesDto[]>(dataSave);

            return Ok(Result<GroupTaxesTaxesDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<GroupTaxesTaxesDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repGroupTaxesTaxes.Find(x => x.IsActive == true
      && (x.GroupTaxes.Description.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).Include(x=> x.Taxes).Include(x=> x.GroupTaxes).ToList();

            int totalRecords = _repGroupTaxesTaxes.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<GroupTaxesTaxesDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<GroupTaxesTaxesDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repGroupTaxesTaxes.GetById(id);

            var mapperOut = _mapper.Map<GroupTaxesTaxesDto>(dataSave);

            return Ok(Result<GroupTaxesTaxesDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repGroupTaxesTaxes.GetById(id);

            data.IsActive = false;

            await _repGroupTaxesTaxes.Update(data);

            var save = await _repGroupTaxesTaxes.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<GroupTaxesTaxesDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<GroupTaxesTaxesDto>(data);

            return Ok(Result<GroupTaxesTaxesDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] GroupTaxesTaxesDto updateDto)
        {
            var mapper = _mapper.Map<GroupTaxesTaxes>(updateDto);
            mapper.IsActive = true;
            var result = await _repGroupTaxesTaxes.Update(mapper);

            var dataSave = await _repGroupTaxesTaxes.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<GroupTaxesTaxesDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<GroupTaxesTaxesDto>(result);

            return Ok(Result<GroupTaxesTaxesDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}