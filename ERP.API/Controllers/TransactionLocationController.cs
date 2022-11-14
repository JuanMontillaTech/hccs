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
    public class TransactionLocationController : ControllerBase
    {
        private readonly IGenericRepository<TransactionLocation> _repTransactionLocation;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int _dataSave;

        public TransactionLocationController(IGenericRepository<TransactionLocation> repTransactionLocation, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repTransactionLocation = repTransactionLocation;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionLocationDto data)
        {

            var mapper = _mapper.Map<TransactionLocation>(data);

            var result = await _repTransactionLocation.Insert(mapper);
            try
            {
                _dataSave = await _repTransactionLocation.SaveChangesAsync();
                if (_dataSave != 1)
                    return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorCreating, "API"));
                var mapperOut = _mapper.Map<TransactionLocationDto>(result);
                return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
            }
            catch (Exception ex)
            {
                var re = ex.Message;
            }

            return Ok(Result<TransactionLocationDto>.Fail("Error al insentar", MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repTransactionLocation.Find(x => x.IsActive).AsQueryable()
                .Include(x => x.Form).ToListAsync();


            var mapperOut = _mapper.Map<TransactionLocationDto[]>(dataSave);

            return Ok(Result<TransactionLocationDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionLocationDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = _repTransactionLocation.Find(x => x.IsActive == true
            && (x.Form.Title.ToLower().Contains(filter.Search.Trim().ToLower()))
            && (x.Form.Label.ToLower().Contains(filter.Search.Trim().ToLower()))
             || (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = _repTransactionLocation.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<TransactionLocationDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionLocationDto>>.Success(List);
            return Ok(Result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repTransactionLocation.GetById(id);

            var mapperOut = _mapper.Map<TransactionLocationDto>(dataSave);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repTransactionLocation.GetById(id);

            data.IsActive = false;

            await _repTransactionLocation.Update(data);

            var save = await _repTransactionLocation.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionLocationDto>(data);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionLocationDto updateDto)
        {
            var mapper = _mapper.Map<TransactionLocation>(updateDto);
            mapper.IsActive = true;
            var result = await _repTransactionLocation.Update(mapper);

            var dataSave = await _repTransactionLocation.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<TransactionLocationDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionLocationDto>(result);

            return Ok(Result<TransactionLocationDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}