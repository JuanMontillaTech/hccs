using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;

using ERP.Domain.Command;
using ERP.Domain.Constants;
using ERP.Domain.Dtos;
using ERP.Domain.Entity;
using ERP.Domain.Filter;
using ERP.Services.Extensions;
using ERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly IGenericRepository<TransactionType> _repTransactionType;

        private readonly IMapper _mapper;

        private int _dataSave;

        public TransactionTypeController(IGenericRepository<TransactionType> repTransactionType, IMapper mapper)
        {
            _repTransactionType = repTransactionType;

            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionTypeDto data)
        {

            var mapper = _mapper.Map<TransactionType>(data);


            var result = await _repTransactionType.InsertAsync(mapper);

            _dataSave = await _repTransactionType.SaveChangesAsync();
            if (_dataSave != 1)
                return Ok(Result<TransactionTypeDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TransactionTypeDto>(result);
            return Ok(Result<TransactionTypeDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));


        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var dataSave = await _repTransactionType.Find(x => x.IsActive).AsQueryable().ToListAsync();


            var mapperOut = _mapper.Map<TransactionTypeDto[]>(dataSave);

            return Ok(Result<TransactionTypeDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionTypeDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var getTransactionType = _repTransactionType.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
             && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))
            ).ToList();

            int totalRecords = getTransactionType.Count();
            var dataMaperOut = _mapper.Map<List<TransactionTypeDto>>(getTransactionType);

            var listTransactionType = dataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var result = Result<PagesPagination<TransactionTypeDto>>.Success(listTransactionType);
            return Ok(result);



        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var dataSave = await _repTransactionType.GetById(id);

            var mapperOut = _mapper.Map<TransactionTypeDto>(dataSave);

            return Ok(Result<TransactionTypeDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _repTransactionType.GetById(id);

            data.IsActive = false;

            await _repTransactionType.Update(data);

            var save = await _repTransactionType.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionTypeDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionTypeDto>(data);

            return Ok(Result<TransactionTypeDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionTypeDto updateDto)
        {
            var mapper = _mapper.Map<TransactionType>(updateDto);
            mapper.IsActive = true;
            var result = await _repTransactionType.Update(mapper);

            var dataSave = await _repTransactionType.SaveChangesAsync();

            if (dataSave != 1)
                return Ok(Result<TransactionTypeDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionTypeDto>(result);

            return Ok(Result<TransactionTypeDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }
    }
}