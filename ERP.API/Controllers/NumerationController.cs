 


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
    public class NumerationController : ControllerBase
    {
        private readonly IGenericRepository<Numeration> _repNumerations;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public NumerationController(IGenericRepository<Numeration> repNumerations, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repNumerations = repNumerations;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] NumerationDto data)
        {

           

            var mapper = _mapper.Map<Numeration>(data);


            var result = await _repNumerations.InsertAsync(mapper);
            try
            {
                _dataSave = await _repNumerations.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<NumerationDto>(result);
                return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<NumerationDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repNumerations.Find(x => x.IsActive).AsQueryable()
           .ToListAsync();


            var mapperOut = _mapper.Map<NumerationDto[]>(dataSave);

            return Ok(Result<NumerationDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<NumerationDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repNumerations.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Prefix.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repNumerations.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<NumerationDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<NumerationDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repNumerations.GetById(id);

            var mapperOut = _mapper.Map<NumerationDto>(dataSave);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repNumerations.GetById(id);

            data.IsActive = false;

            await _repNumerations.Update(data);

            var save = await _repNumerations.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<NumerationDto>(data);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] NumerationDto updateDto)
        {
            var mapper = _mapper.Map<Numeration>(updateDto);
            mapper.IsActive = true;
            var result = await _repNumerations.Update(mapper);

            var dataSave = await _repNumerations.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<NumerationDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<NumerationDto>(result);

            return Ok(Result<NumerationDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}
