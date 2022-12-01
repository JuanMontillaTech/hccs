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
    public class TransactionsDetailsElementTypeController : ControllerBase
    {
        public readonly IGenericRepository<TransactionsDetailsElementType> RepTransactionsDetailsElementTypes;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionsDetailsElementTypeController(IGenericRepository<TransactionsDetailsElementType> repTransactionsDetailsElementTypes, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepTransactionsDetailsElementTypes = repTransactionsDetailsElementTypes;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDetailsElementTypeDto data)
        {
            var mapper = _mapper.Map<TransactionsDetailsElementType>(data);

            var result = await RepTransactionsDetailsElementTypes.InsertAsync(mapper);

            var DataSave = await RepTransactionsDetailsElementTypes.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionsDetailsElementTypeDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TransactionsDetailsElementTypeDto>(result);

            return Ok(Result<TransactionsDetailsElementTypeDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var Filter = await RepTransactionsDetailsElementTypes.Find(x => x.IsActive == true).OrderBy(x=> x.Name).ToListAsync();

            var mapperOut = _mapper.Map<TransactionsDetailsElementTypeDto[]>(Filter);

            return Ok(Result<TransactionsDetailsElementTypeDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionsDetailsElementTypeDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepTransactionsDetailsElementTypes.Find(x => x.IsActive == true
            && (x.Name.ToLower().Contains(filter.Search.Trim().ToLower()))).ToList();

            int totalRecords = RepTransactionsDetailsElementTypes.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<TransactionsDetailsElementTypeDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionsDetailsElementTypeDto>>.Success(List);
            return Ok(Result);



        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var DataSave = await RepTransactionsDetailsElementTypes.GetById(id);

            var mapperOut = _mapper.Map<TransactionsDetailsElementTypeDto>(DataSave);

            return Ok(Result<TransactionsDetailsElementTypeDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTransactionsDetailsElementTypes.GetById(id);

            Data.IsActive = false;

            await RepTransactionsDetailsElementTypes.Update(Data);

            var save = await RepTransactionsDetailsElementTypes.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionsDetailsElementTypeDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionsDetailsElementTypeDto>(Data);

            return Ok(Result<TransactionsDetailsElementTypeDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionsDetailsElementTypeDto _UpdateDto)
        {

            var mapper = _mapper.Map<TransactionsDetailsElementType>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTransactionsDetailsElementTypes.Update(mapper);

            var DataSave = await RepTransactionsDetailsElementTypes.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionsDetailsElementTypeDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionsDetailsElementTypeDto>(result);

            return Ok(Result<TransactionsDetailsElementTypeDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
