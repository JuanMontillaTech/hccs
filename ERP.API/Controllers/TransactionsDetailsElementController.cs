using Amazon.S3.Model;
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
    public class TransactionsDetailsElementController : ControllerBase
    {
        public readonly IGenericRepository<TransactionsDetailsElement> RepTransactionsDetailsElements;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionsDetailsElementController(IGenericRepository<TransactionsDetailsElement> repTransactionsDetailsElements, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            RepTransactionsDetailsElements = repTransactionsDetailsElements;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionsDetailsElementDto data)
        {
            var mapper = _mapper.Map<TransactionsDetailsElement>(data);

            var result = await RepTransactionsDetailsElements.InsertAsync(mapper);

            var DataSave = await RepTransactionsDetailsElements.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionsDetailsElementDto>.Fail(MessageCodes.ErrorCreating, "API"));
            var mapperOut = _mapper.Map<TransactionsDetailsElementDto>(result);

            return Ok(Result<TransactionsDetailsElementDto>.Success(mapperOut, MessageCodes.AddedSuccessfully()));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            
            var Filter = await RepTransactionsDetailsElements.Find(x => x.IsActive == true).Include(x=> x.TransactionsDetailsElementType).OrderBy(x=> x.Detaills).ToListAsync();

            var mapperOut = _mapper.Map<TransactionsDetailsElementDto[]>(Filter);

            return Ok(Result<TransactionsDetailsElementDto[]>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }
        [HttpGet("GetFilter")]
        [ProducesResponseType(typeof(Result<ICollection<TransactionsDetailsElementDto>>), (int)HttpStatusCode.OK)]

        public IActionResult GetFilter([FromQuery] PaginationFilter filter)
        {

            var Filter = RepTransactionsDetailsElements.Find(x => x.IsActive == true
            && (x.Detaills.ToLower().Contains(filter.Search.Trim().ToLower()))).Include(x => x.TransactionsDetailsElementType).ToList();

            int totalRecords = RepTransactionsDetailsElements.Find(t => t.IsActive).Count();
            var DataMaperOut = _mapper.Map<List<TransactionsDetailsElementDto>>(Filter);

            var List = DataMaperOut.AsQueryable().PaginationPages(filter, totalRecords);
            var Result = Result<PagesPagination<TransactionsDetailsElementDto>>.Success(List);
            return Ok(Result);



        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            
            var DataSave = await RepTransactionsDetailsElements.Find(x => x.IsActive == true
            && x.Id == id).Include(x => x.TransactionsDetailsElementType).ToListAsync();

            var mapperOut = _mapper.Map<TransactionsDetailsElementDto>(DataSave);

            return Ok(Result<TransactionsDetailsElementDto>.Success(mapperOut, MessageCodes.AllSuccessfully()));
        }

        

        [HttpDelete("Delete/{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var Data = await RepTransactionsDetailsElements.GetById(id);

            Data.IsActive = false;

            await RepTransactionsDetailsElements.Update(Data);

            var save = await RepTransactionsDetailsElements.SaveChangesAsync();

            if (save != 1)
                return Ok(Result<TransactionsDetailsElementDto>.Fail(MessageCodes.ErrorDeleting, "API"));

            var mapperOut = _mapper.Map<TransactionsDetailsElementDto>(Data);

            return Ok(Result<TransactionsDetailsElementDto>.Success(mapperOut, MessageCodes.InactivatedSuccessfully()));
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionsDetailsElementDto _UpdateDto)
        {

            var mapper = _mapper.Map<TransactionsDetailsElement>(_UpdateDto);
            mapper.IsActive = true;
            var result = await RepTransactionsDetailsElements.Update(mapper);

            var DataSave = await RepTransactionsDetailsElements.SaveChangesAsync();

            if (DataSave != 1)
                return Ok(Result<TransactionsDetailsElementDto>.Fail(MessageCodes.ErrorUpdating, "API"));

            var mapperOut = _mapper.Map<TransactionsDetailsElementDto>(result);

            return Ok(Result<TransactionsDetailsElementDto>.Success(mapperOut, MessageCodes.UpdatedSuccessfully()));
        }

    }



}
